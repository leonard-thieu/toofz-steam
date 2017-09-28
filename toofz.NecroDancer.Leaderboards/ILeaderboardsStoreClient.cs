using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SqlBulkUpsert;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsStoreClient
    {
        ColumnMappings<Leaderboard> GetLeaderboardMappings();
        Task<int> SaveChangesAsync(
            IEnumerable<Leaderboard> leaderboards,
            CancellationToken cancellationToken = default);

        ColumnMappings<Entry> GetEntryMappings();
        Task<int> SaveChangesAsync(
            IEnumerable<Entry> entries,
            CancellationToken cancellationToken = default);

        ColumnMappings<DailyLeaderboard> GetDailyLeaderboardMappings();
        Task<int> SaveChangesAsync(
            IEnumerable<DailyLeaderboard> leaderboards,
            CancellationToken cancellationToken = default);

        ColumnMappings<DailyEntry> GetDailyEntryMappings();
        Task<int> SaveChangesAsync(
            IEnumerable<DailyEntry> entries,
            CancellationToken cancellationToken = default);

        ColumnMappings<Player> GetPlayerMappings();
        Task<int> SaveChangesAsync(
            IEnumerable<Player> players,
            bool updateOnMatch,
            CancellationToken cancellationToken = default);

        ColumnMappings<Replay> GetReplayMappings();
        Task<int> SaveChangesAsync(
            IEnumerable<Replay> replays,
            bool updateOnMatch,
            CancellationToken cancellationToken = default);
    }
}