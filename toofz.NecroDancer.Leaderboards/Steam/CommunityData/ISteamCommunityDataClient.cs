using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    public interface ISteamCommunityDataClient : IDisposable
    {
        Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            uint appId,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            string communityGameName,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<Entry>> GetLeaderboardEntriesAsync(
            uint appId,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<Entry>> GetLeaderboardEntriesAsync(
            string communityGameName,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
    }
}