using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    public interface ISteamCommunityDataClient : IDisposable
    {
        Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            uint appId,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default);
        Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            string communityGameName,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default);
        Task<LeaderboardEntriesEnvelope> GetLeaderboardEntriesAsync(
            uint appId,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default);
        Task<LeaderboardEntriesEnvelope> GetLeaderboardEntriesAsync(
            string communityGameName,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default);
    }
}