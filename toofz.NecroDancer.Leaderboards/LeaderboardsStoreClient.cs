using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using SqlBulkUpsert;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsStoreClient : ILeaderboardsStoreClient
    {
        #region Leaderboard

        public ColumnMappings<Leaderboard> GetLeaderboardMappings() => new ColumnMappings<Leaderboard>("Leaderboards")
        {
            d => d.LeaderboardId,
            d => d.LastUpdate,
            d => d.CharacterId,
            d => d.RunId,
            d => d.Date,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Leaderboard> leaderboards,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var upserter = new TypedUpserter<Leaderboard>(GetLeaderboardMappings());

            return SaveChangesAsync(upserter, connection, leaderboards, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Leaderboard> upserter,
            SqlConnection connection,
            IEnumerable<Leaderboard> leaderboards,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return UpsertAsync(upserter, connection, leaderboards, true, cancellationToken);
        }

        #endregion

        #region Entry

        public ColumnMappings<Entry> GetEntryMappings() => new ColumnMappings<Entry>("Entries")
        {
            d => d.LeaderboardId,
            d => d.Rank,
            d => d.SteamId,
            d => d.ReplayId,
            d => d.Score,
            d => d.Zone,
            d => d.Level,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Entry> entries,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var upserter = new TypedUpserter<Entry>(GetEntryMappings());

            return SaveChangesAsync(upserter, connection, entries, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Entry> upserter,
            SqlConnection connection,
            IEnumerable<Entry> entries,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return upserter.InsertAsync(connection, entries, cancellationToken);
        }

        #endregion

        #region DailyLeaderboard

        public ColumnMappings<DailyLeaderboard> GetDailyLeaderboardMappings() => new ColumnMappings<DailyLeaderboard>("DailyLeaderboards")
        {
            d => d.LeaderboardId,
            d => d.LastUpdate,
            d => d.Date,
            d => d.ProductId,
            d => d.IsProduction
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<DailyLeaderboard> leaderboards,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var upserter = new TypedUpserter<DailyLeaderboard>(GetDailyLeaderboardMappings());

            return SaveChangesAsync(upserter, connection, leaderboards, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<DailyLeaderboard> upserter,
            SqlConnection connection,
            IEnumerable<DailyLeaderboard> leaderboards,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return UpsertAsync(upserter, connection, leaderboards, true, cancellationToken);
        }

        #endregion

        #region DailyEntry

        public ColumnMappings<DailyEntry> GetDailyEntryMappings() => new ColumnMappings<DailyEntry>("DailyEntries")
        {
            d => d.LeaderboardId,
            d => d.Rank,
            d => d.SteamId,
            d => d.ReplayId,
            d => d.Score,
            d => d.Zone,
            d => d.Level,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<DailyEntry> entries,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var upserter = new TypedUpserter<DailyEntry>(GetDailyEntryMappings());

            return SaveChangesAsync(upserter, connection, entries, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<DailyEntry> upserter,
            SqlConnection connection,
            IEnumerable<DailyEntry> entries,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return UpsertAsync(upserter, connection, entries, true, cancellationToken);
        }

        #endregion

        #region Player

        public ColumnMappings<Player> GetPlayerMappings() => new ColumnMappings<Player>("Players")
        {
            d => d.SteamId,
            d => d.Exists,
            d => d.Name,
            d => d.LastUpdate,
            d => d.Avatar,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Player> players,
            bool updateOnMatch,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var upserter = new TypedUpserter<Player>(GetPlayerMappings());

            return SaveChangesAsync(upserter, connection, players, updateOnMatch, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Player> upserter,
            SqlConnection connection,
            IEnumerable<Player> players,
            bool updateOnMatch,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return UpsertAsync(upserter, connection, players, updateOnMatch, cancellationToken);
        }

        #endregion

        #region Replay

        public ColumnMappings<Replay> GetReplayMappings() => new ColumnMappings<Replay>("Replays")
        {
            d => d.ReplayId,
            d => d.ErrorCode,
            d => d.Seed,
            d => d.KilledBy,
            d => d.Version,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            SqlConnection connection,
            IEnumerable<Replay> replays,
            bool updateOnMatch,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var upserter = new TypedUpserter<Replay>(GetReplayMappings());

            return SaveChangesAsync(upserter, connection, replays, updateOnMatch, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Replay> upserter,
            SqlConnection connection,
            IEnumerable<Replay> replays,
            bool updateOnMatch,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return UpsertAsync(upserter, connection, replays, updateOnMatch, cancellationToken);
        }

        #endregion

        Task<int> UpsertAsync<T>(
           ITypedUpserter<T> upserter,
           SqlConnection connection,
           IEnumerable<T> items,
           bool updateOnMatch,
           CancellationToken cancellationToken)
        {
            return upserter.UpsertAsync(connection, items, updateOnMatch, cancellationToken);
        }
    }
}