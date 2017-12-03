using System;
using System.Threading;
using System.Threading.Tasks;
using static SteamKit2.SteamUserStats;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public interface ISteamClientApiClient : IDisposable
    {
        /// <summary>
        /// Gets or sets an instance of <see cref="IProgress{T}"/> that is used to report total bytes downloaded.
        /// </summary>
        IProgress<long> Progress { get; set; }
        /// <summary>
        /// Gets or sets the period of time before jobs will be considered timed out and will be canceled.
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Connects and logs on to Steam.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task representing connecting and logging on to Steam.
        /// </returns>
        Task ConnectAndLogOnAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Disconnects from Steam.
        /// </summary>
        void Disconnect();
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
        Task<IFindOrCreateLeaderboardCallback> FindLeaderboardAsync(
            uint appId,
            string name,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets leaderboard entries for the specified AppID and leaderboard ID.
        /// </summary>
        /// <param name="appId">The AppID of the leaderboard.</param>
        /// <param name="leaderboardId">The leaderboard ID of the leaderboard.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="SteamClientApiException">
        /// Unable to retrieve entries for leaderboard.
        /// </exception>
        Task<ILeaderboardEntriesCallback> GetLeaderboardEntriesAsync(
            uint appId,
            int leaderboardId,
            CancellationToken cancellationToken = default);
    }
}