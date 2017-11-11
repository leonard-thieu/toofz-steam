using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using SteamKit2;
using static SteamKit2.SteamUser;
using static SteamKit2.SteamUserStats;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public sealed class SteamClientApiClient : ISteamClientApiClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientApiClient));

        private static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        private static readonly RetryPolicy RetryPolicy = SteamClientApiTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

        private static ISteamClientAdapter CreateSteamClient()
        {
            var steamClient = new SteamClient { DebugNetworkListener = new ProgressDebugNetworkListener() };
            var manager = new CallbackManager(steamClient);

            return new SteamClientAdapter(steamClient, manager);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiClient"/> class 
        /// with the specified user name and password.
        /// </summary>
        /// <param name="userName">The user name to log on to Steam with.</param>
        /// <param name="password">The password to log on to Steam with.</param>
        /// <param name="telemetryClient">The telemetry client to use for reporting telemetry.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="userName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="userName"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public SteamClientApiClient(string userName, string password, TelemetryClient telemetryClient) : this(userName, password, telemetryClient, CreateSteamClient()) { }

        internal SteamClientApiClient(string userName, string password, TelemetryClient telemetryClient, ISteamClientAdapter steamClient)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));
            if (userName == "")
                throw new ArgumentException($"{nameof(userName)} is empty.", nameof(userName));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password == "")
                throw new ArgumentException($"{nameof(password)} is empty.", nameof(password));

            this.userName = userName;
            this.password = password;
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            this.steamClient = steamClient;
        }

        private readonly string userName;
        private readonly string password;
        private readonly TelemetryClient telemetryClient;
        private readonly ISteamClientAdapter steamClient;

        /// <summary>
        /// Gets or sets an instance of <see cref="IProgress{T}"/> that is used to report total bytes downloaded.
        /// </summary>
        public IProgress<long> Progress
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(SteamClientApiClient));

                return steamClient.ProgressDebugNetworkListener.Progress;
            }
            set
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(SteamClientApiClient));

                steamClient.ProgressDebugNetworkListener.Progress = value;
            }
        }

        /// <summary>
        /// Gets or sets the period of time before jobs will be considered timed out and will be canceled. 
        /// By default this is 10 seconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        #region Connection

        /// <summary>
        /// Connects and logs on to Steam.
        /// </summary>
        /// <returns>
        /// The task representing connecting and logging on to Steam.
        /// </returns>
        public async Task ConnectAndLogOnAsync()
        {
            if (!steamClient.IsConnected)
            {
                await steamClient
                    .ConnectAsync()
                    .TimeoutAfter(Timeout)
                    .ConfigureAwait(false);
            }
            if (!steamClient.IsLoggedOn)
            {
                await steamClient
                    .LogOnAsync(new LogOnDetails
                    {
                        Username = userName,
                        Password = password,
                    })
                    .TimeoutAfter(Timeout)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Disconnects from Steam.
        /// </summary>
        public void Disconnect() => steamClient.Disconnect();

        #endregion

        /// <summary>
        /// Gets the leaderboard for the specified AppID and name.
        /// </summary>
        /// <exception cref="SteamClientApiException">
        /// Unable to find the leaderboard.
        /// </exception>
        /// <exception cref="SteamClientApiException">
        /// Unable to retrieve the leaderboard.
        /// </exception>
        public async Task<IFindOrCreateLeaderboardCallback> FindLeaderboardAsync(
            uint appId,
            string name,
            CancellationToken cancellationToken = default)
        {
            var request = steamClient.GetSteamUserStats().FindLeaderboard(appId, name);
            var response = await ExecuteRequestAsync(request, name, cancellationToken).ConfigureAwait(false);

            if (response.Result != EResult.OK || response.ID == 0)
                throw new SteamClientApiException($"Unable to find the leaderboard '{name}'.", response.Result);

            return response;
        }

        /// <summary>
        /// Gets leaderboard entries for the specified AppID and leaderboard ID.
        /// </summary>
        /// <exception cref="SteamClientApiException">
        /// Unable to retrieve entries for leaderboard.
        /// </exception>
        public async Task<ILeaderboardEntriesCallback> GetLeaderboardEntriesAsync(
            uint appId,
            int lbid,
            CancellationToken cancellationToken = default)
        {
            var request = steamClient.GetSteamUserStats().GetLeaderboardEntries(appId, lbid, 0, int.MaxValue, ELeaderboardDataRequest.Global);
            var response = await ExecuteRequestAsync(request, lbid.ToString(), cancellationToken).ConfigureAwait(false);

            if (response.Result != EResult.OK)
                throw new SteamClientApiException($"Unable to get leaderboard entries for '{lbid}'.", response.Result);

            return response;
        }

        // TODO: Make this configurable.
        private static readonly SemaphoreSlim requestSemaphore = new SemaphoreSlim(8 * Environment.ProcessorCount, 8 * Environment.ProcessorCount);

        private async Task<TResult> ExecuteRequestAsync<TResult>(
            IAsyncJob<TResult> request,
            string requestName,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "")
            where TResult : ICallbackMsg
        {
            using (var operation = telemetryClient.StartOperation<DependencyTelemetry>(memberName))
            {
                operation.Telemetry.Type = "Steam3";
                operation.Telemetry.Data = requestName;

                try
                {
                    Log.Debug($"Start download {requestName}");
                    var response = await RetryPolicy
                        .ExecuteAsync(() => ExecuteRequestAsync(request, cancellationToken), cancellationToken)
                        .ConfigureAwait(false);
                    Log.Debug($"End download {requestName}");
                    operation.Telemetry.Success = true;

                    return response;
                }
                catch (Exception ex)
                {
                    telemetryClient.TrackException(ex);
                    operation.Telemetry.Success = false;
                    throw;
                }
            }
        }

        private async Task<TResult> ExecuteRequestAsync<TResult>(
            IAsyncJob<TResult> request,
            CancellationToken cancellationToken)
            where TResult : ICallbackMsg
        {
            await requestSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                EnsureConnectedAndLoggedOn();

                var telemetry = new DependencyTelemetry();
                telemetry.Type = "Steam3";
                telemetry.Timestamp = DateTimeOffset.UtcNow;
                var timer = Stopwatch.StartNew();
                try
                {
                    request.Timeout = Timeout;

                    var response = await request
                        .ToTask()
                        .TimeoutAfter(Timeout) // Used in case SteamKit deadlocks
                        .ConfigureAwait(false);

                    telemetry.Success = true;

                    return response;
                }
                catch (TaskCanceledException ex)
                {
                    telemetry.Success = false;

                    throw new SteamClientApiException("Request timed out.", ex);
                }
                finally
                {
                    timer.Stop();
                    telemetry.Duration = timer.Elapsed;
                    telemetryClient.TrackDependency(telemetry);
                }
            }
            finally
            {
                requestSemaphore.Release();
            }
        }

        private void EnsureConnectedAndLoggedOn()
        {
            if (!steamClient.IsConnected)
                throw new InvalidOperationException("Not connected to Steam.");
            if (!steamClient.IsLoggedOn)
                throw new InvalidOperationException("Not logged on to Steam.");
        }

        #region IDisposable Implementation

        private bool disposed;

        /// <summary>
        /// Disconnects from Steam.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }

            steamClient.Dispose();

            disposed = true;
        }

        #endregion
    }
}
