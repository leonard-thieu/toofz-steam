using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    public interface ISteamCommunityDataClient : IDisposable
    {
        Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            string communityGameName,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        Task<LeaderboardEntriesEnvelope> GetLeaderboardEntriesAsync(
            string communityGameName,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
    }
}