using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public sealed class SteamClientApiClient : ISteamClientApiClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientApiClient));

        static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        static readonly RetryPolicy<SteamClientApiTransientErrorDetectionStrategy> RetryPolicy = SteamClientApiTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

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
        public SteamClientApiClient(string userName, string password) : this(userName, password, new CallbackManagerAdapter()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiClient"/> class 
        /// with the specified user name and password.
        /// </summary>
        /// <param name="userName">The user name to log on to Steam with.</param>
        /// <param name="password">The password to log on to Steam with.</param>
        /// <param name="manager">
        /// The callback manager associated with this instance. If <paramref name="manager"/> is null, a default callback manager 
        /// will be created.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="userName"/> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password"/> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="manager"/>.<see cref="ICallbackManager.SteamClient"/> is null.
        /// </exception>
        internal SteamClientApiClient(string userName, string password, ICallbackManager manager)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException($"{nameof(userName)} is null or empty.", nameof(userName));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"{nameof(password)} is null or empty.", nameof(password));

            this.userName = userName;
            this.password = password;

            steamClient = manager.SteamClient;
            steamClient.ProgressDebugNetworkListener = new ProgressDebugNetworkListener();
        }

        readonly string userName;
        readonly string password;
        readonly ISteamClient steamClient;

        /// <summary>
        /// Gets or sets an instance of <see cref="IProgress{T}"/> that is used to report total bytes downloaded.
        /// </summary>
        public IProgress<long> Progress
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(SteamClientApiClient));

                return steamClient.ProgressDebugNetworkListener?.Progress;
            }
            set
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(SteamClientApiClient));
                if (steamClient.ProgressDebugNetworkListener == null)
                    throw new InvalidOperationException($"{nameof(steamClient)}.{nameof(steamClient.ProgressDebugNetworkListener)} is null.");

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
                await steamClient.ConnectAsync().ConfigureAwait(false);
            }
            if (!steamClient.IsLoggedOn)
            {
                await steamClient.LogOnAsync(new SteamUser.LogOnDetails
                {
                    Username = userName,
                    Password = password,
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Disconnects from Steam.
        /// </summary>
        public void Disconnect()
        {
            steamClient.Disconnect();
        }

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
            var leaderboard =
                await RetryPolicy.ExecuteAsync(async () =>
                {
                    EnsureConnectedAndLoggedOn();
                    try
                    {
                        var steamUserStats = steamClient.GetSteamUserStats();
                        steamUserStats.Timeout = Timeout;

                        using (var downloadNotifier = new DownloadNotifier(Log, name))
                        {
                            return await steamUserStats
                                .FindLeaderboard(appId, name)
                                .ConfigureAwait(false);
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        throw new SteamClientApiException($"Unable to find the leaderboard '{name}' due to timeout.", ex);
                    }
                }, cancellationToken)
                .ConfigureAwait(false);

            switch (leaderboard.Result)
            {
                case EResult.OK: return leaderboard;
                default:
                    throw new SteamClientApiException($"Unable to find the leaderboard '{name}'.", leaderboard.Result);
            }
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
            var leaderboardEntries =
                await RetryPolicy.ExecuteAsync(async () =>
                {
                    EnsureConnectedAndLoggedOn();
                    try
                    {
                        var steamUserStats = steamClient.GetSteamUserStats();
                        steamUserStats.Timeout = Timeout;

                        using (var downloadNotifier = new DownloadNotifier(Log, $"{lbid}"))
                        {
                            return await steamUserStats
                                .GetLeaderboardEntries(appId, lbid, 0, int.MaxValue, ELeaderboardDataRequest.Global)
                                .ConfigureAwait(false);
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        throw new SteamClientApiException($"Unable to retrieve entries for leaderboard '{lbid}' due to timeout.", ex);
                    }
                }, cancellationToken)
                .ConfigureAwait(false);

            switch (leaderboardEntries.Result)
            {
                case EResult.OK: return leaderboardEntries;
                default:
                    throw new SteamClientApiException($"Unable to retrieve entries for leaderboard '{lbid}'.", leaderboardEntries.Result);
            }
        }

        void EnsureConnectedAndLoggedOn()
        {
            if (!steamClient.IsConnected)
                throw new InvalidOperationException("Not connected to Steam.");
            if (!steamClient.IsLoggedOn)
                throw new InvalidOperationException("Not logged on to Steam.");
        }

        #region IDisposable Implementation

        bool disposed;

        /// <summary>
        /// Disconnects from Steam.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                if (steamClient.IsConnected)
                {
                    steamClient.Disconnect();
                }

                disposed = true;
            }
        }

        #endregion
    }
}
