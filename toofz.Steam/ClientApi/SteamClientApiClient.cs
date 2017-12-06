using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Polly;
using Polly.Timeout;
using SteamKit2;
using static SteamKit2.SteamUser;
using static SteamKit2.SteamUserStats;

namespace toofz.Steam.ClientApi
{
    public sealed class SteamClientApiClient : ISteamClientApiClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientApiClient));

        /// <summary>
        /// Indicates if an exception is a transient fault for <see cref="SteamClientApiClient"/>.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>
        /// true, if the exception is a transient fault for <see cref="SteamClientApiClient"/>; otherwise, false.
        /// </returns>
        public static bool IsTransient(Exception ex)
        {
            if (ex is SteamClientApiException scae)
            {
                return scae.Result == null;
            }

            return ex is TimeoutRejectedException;
        }

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
        /// <param name="retryPolicy">The transient fault handling policy used for sending requests.</param>
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
        /// <paramref name="retryPolicy"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public SteamClientApiClient(string userName, string password, Policy retryPolicy, TelemetryClient telemetryClient)
            : this(userName, password, retryPolicy, telemetryClient, steamClient: null) { }

        internal SteamClientApiClient(string userName, string password, Policy retryPolicy, TelemetryClient telemetryClient, ISteamClientAdapter steamClient)
        {
            if (userName == "")
                throw new ArgumentException($"{nameof(userName)} is empty.", nameof(userName));
            if (password == "")
                throw new ArgumentException($"{nameof(password)} is empty.", nameof(password));

            this.userName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            this.steamClient = steamClient ?? CreateSteamClient();

            timeoutPolicy = Policy.TimeoutAsync(() => Timeout + SteamClientAdapter.FallbackTimeoutPadding, TimeoutStrategy.Pessimistic);
        }

        private readonly string userName;
        private readonly string password;
        private readonly Policy retryPolicy;
        private readonly TelemetryClient telemetryClient;
        private readonly ISteamClientAdapter steamClient;

        private readonly Policy timeoutPolicy;

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
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task representing connecting and logging on to Steam.
        /// </returns>
        public Task ConnectAndLogOnAsync(CancellationToken cancellationToken = default)
        {
            var connectPolicy = Policy
                .Handle<Exception>(IsTransient)
                .WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(5) });

            return ConnectAndLogOnAsync(connectPolicy, cancellationToken);
        }

        internal async Task ConnectAndLogOnAsync(
            Policy connectPolicy,
            CancellationToken cancellationToken = default)
        {
            if (!steamClient.IsConnected)
            {
                await connectPolicy
                    .ExecuteAsync(steamClient.ConnectAsync, cancellationToken, continueOnCapturedContext: false)
                    .ConfigureAwait(false);
            }

            if (!steamClient.IsLoggedOn)
            {
                await steamClient.LogOnAsync(new LogOnDetails
                {
                    Username = userName,
                    Password = password,
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Disconnects from Steam.
        /// </summary>
        public void Disconnect() => steamClient.Disconnect();

        private void EnsureConnectedAndLoggedOn()
        {
            if (!steamClient.IsConnected)
                throw new InvalidOperationException("Not connected to Steam.");
            if (!steamClient.IsLoggedOn)
                throw new InvalidOperationException("Not logged on to Steam.");
        }

        #endregion

        /// <summary>
        /// Gets the leaderboard for the specified AppID and name.
        /// </summary>
        /// <param name="appId">The AppID of the leaderboard.</param>
        /// <param name="name">The name of the leaderboard.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
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
            var response = await ExecuteRequestAsync("Find leaderboard", request, name, cancellationToken).ConfigureAwait(false);

            if (response.Result != EResult.OK || response.ID == 0)
                throw new SteamClientApiException($"Unable to find the leaderboard '{name}'.", response.Result);

            return response;
        }

        /// <summary>
        /// Gets leaderboard entries for the specified AppID and leaderboard ID.
        /// </summary>
        /// <param name="appId">The AppID of the leaderboard.</param>
        /// <param name="leaderboardId">The leaderboard ID of the leaderboard.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="SteamClientApiException">
        /// Unable to retrieve entries for leaderboard.
        /// </exception>
        public async Task<ILeaderboardEntriesCallback> GetLeaderboardEntriesAsync(
            uint appId,
            int leaderboardId,
            CancellationToken cancellationToken = default)
        {
            var request = steamClient.GetSteamUserStats().GetLeaderboardEntries(appId, leaderboardId, 0, int.MaxValue, ELeaderboardDataRequest.Global);
            var response = await ExecuteRequestAsync("Get leaderboard entries", request, leaderboardId.ToString(), cancellationToken).ConfigureAwait(false);

            if (response.Result != EResult.OK)
                throw new SteamClientApiException($"Unable to get leaderboard entries for '{leaderboardId}'.", response.Result);

            return response;
        }

        // TODO: Make this configurable.
        private static readonly SemaphoreSlim requestSemaphore = new SemaphoreSlim(8 * Environment.ProcessorCount, 8 * Environment.ProcessorCount);

        private async Task<TResult> ExecuteRequestAsync<TResult>(
            string operationName,
            IAsyncJob<TResult> request,
            string requestName,
            CancellationToken cancellationToken)
            where TResult : ICallbackMsg
        {
            using (var operation = telemetryClient.StartOperation<DependencyTelemetry>(operationName))
            {
                var telemetry = operation.Telemetry;
                telemetry.Type = "Steam3";
                telemetry.Target = steamClient.RemoteIP?.ToString();
                telemetry.Data = requestName;

                try
                {
                    if (Log.IsDebugEnabled) { Log.Debug($"Start download {operationName} {requestName}"); }
                    var response = await retryPolicy
                        .ExecuteAsync(cancellation => ExecuteRequestCoreAsync(operationName, request, requestName, cancellation), cancellationToken)
                        .ConfigureAwait(false);
                    if (Log.IsDebugEnabled) { Log.Debug($"End download {operationName} {requestName}"); }
                    telemetry.Success = true;

                    return response;
                }
                catch (Exception) when (operation.Telemetry.MarkAsUnsuccessful())
                {
                    // Unreachable
                    throw;
                }
            }
        }

        private async Task<TResult> ExecuteRequestCoreAsync<TResult>(
            string operationName,
            IAsyncJob<TResult> request,
            string requestName,
            CancellationToken cancellationToken)
            where TResult : ICallbackMsg
        {
            await requestSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                EnsureConnectedAndLoggedOn();

                var telemetry = new DependencyTelemetry { Name = operationName };
                telemetry.Type = "Steam3";
                telemetry.Target = steamClient.RemoteIP?.ToString();
                telemetry.Data = requestName;
                telemetry.Timestamp = DateTimeOffset.UtcNow;
                var timer = Stopwatch.StartNew();

                try
                {
                    request.Timeout = Timeout;

                    // A fallback timeout is used in case SteamKit deadlocks.
                    var response = await timeoutPolicy
                        .ExecuteAsync(request.ToTask, continueOnCapturedContext: false)
                        .ConfigureAwait(false);

                    telemetry.Success = true;

                    return response;
                }
                catch (TaskCanceledException ex)
                {
                    telemetry.Success = false;
                    throw new SteamClientApiException("Request timed out.", ex);
                }
                catch (Exception) when (telemetry.MarkAsUnsuccessful())
                {
                    // Unreachable
                    throw;
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

        #region IDisposable Implementation

        private bool disposed;

        /// <summary>
        /// Disposes of resources used by <see cref="SteamClientApiClient"/>.
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
