using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using SqlBulkUpsert;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsStoreClient
    {
        ColumnMappings<Leaderboard> GetLeaderboardMappings();
        Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Leaderboard> leaderboards,
            CancellationToken cancellationToken = default(CancellationToken));

        ColumnMappings<Entry> GetEntryMappings();
        Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Entry> entries,
            CancellationToken cancellationToken = default(CancellationToken));

        ColumnMappings<DailyLeaderboard> GetDailyLeaderboardMappings();
        Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<DailyLeaderboard> leaderboards,
            CancellationToken cancellationToken = default(CancellationToken));

        ColumnMappings<DailyEntry> GetDailyEntryMappings();
        Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<DailyEntry> entries,
            CancellationToken cancellationToken = default(CancellationToken));

        ColumnMappings<Player> GetPlayerMappings();
        Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Player> players,
            bool updateOnMatch,
            CancellationToken cancellationToken = default(CancellationToken));

        ColumnMappings<Replay> GetReplayMappings();
        Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Replay> replays,
            bool updateOnMatch,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}