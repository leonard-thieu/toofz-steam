using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
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
        /// <exception cref="ArgumentException">
        /// <paramref name="userName"/> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password"/> is null or empty.
        /// </exception>
        public SteamClientApiClient(string userName, string password) : this(userName, password, CreateSteamClient()) { }

        internal SteamClientApiClient(string userName, string password, ISteamClientAdapter steamClient)
        {
            // TODO: Consider having separate checks for null that throw ArgumentNullException.
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException($"{nameof(userName)} is null or empty.", nameof(userName));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"{nameof(password)} is null or empty.", nameof(password));

            this.userName = userName;
            this.password = password;
            this.steamClient = steamClient;
        }

        private readonly string userName;
        private readonly string password;
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

            // TODO: This should check if ID is not 0.
            if (response.Result != EResult.OK)
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
            CancellationToken cancellationToken)
            where TResult : ICallbackMsg
        {
            Log.Debug($"Start download {requestName}");

            var response = await RetryPolicy.ExecuteAsync(async () =>
            {
                await requestSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    EnsureConnectedAndLoggedOn();

                    try
                    {
                        request.Timeout = Timeout;

                        return await request
                            .ToTask()
                            .TimeoutAfter(Timeout) // Used in case SteamKit deadlocks
                            .ConfigureAwait(false);
                    }
                    catch (TaskCanceledException ex)
                    {
                        throw new SteamClientApiException("Request timed out.", ex);
                    }
                }
                finally
                {
                    requestSemaphore.Release();
                }
            }, cancellationToken).ConfigureAwait(false);

            Log.Debug($"End download {requestName}");

            return response;
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
