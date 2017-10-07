using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public interface ISteamClientApiClient : IDisposable
    {
        /// <summary>
        /// Gets or sets an instance of <see cref="IProgress{T}"/> that is used to report total bytes downloaded.
        /// </summary>
        IProgress<long> Progress { get; set; }

        /// <summary>
        /// Gets leaderboard entries for the specified AppID and leaderboard ID.
        /// </summary>
        /// <exception cref="SteamClientApiException">
        /// Unable to retrieve entries for leaderboard.
        /// </exception>
        Task<IFindOrCreateLeaderboardCallback> FindLeaderboardAsync(
            uint appId,
            string name,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the leaderboard for the specified AppID and name.
        /// </summary>
        /// <exception cref="SteamClientApiException">
        /// Unable to find the leaderboard.
        /// </exception>
        /// <exception cref="SteamClientApiException">
        /// Unable to retrieve the leaderboard.
        /// </exception>
        Task<ILeaderboardEntriesCallback> GetLeaderboardEntriesAsync(
            uint appId,
            int lbid,
            CancellationToken cancellationToken = default);
    }
}