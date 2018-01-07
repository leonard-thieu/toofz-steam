using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// HTTP client used for interacting with Steam Community Data.
    /// </summary>
    public interface ISteamCommunityDataClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="communityGameName"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            string communityGameName,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="communityGameName"></param>
        /// <param name="leaderboardId"></param>
        /// <param name="params"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LeaderboardEntriesEnvelope> GetLeaderboardEntriesAsync(
            string communityGameName,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
    }
}