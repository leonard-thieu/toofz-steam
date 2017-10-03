using System;
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
        public LeaderboardsStoreClient(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        readonly SqlConnection connection;

        #region Leaderboard

        public ColumnMappings<Leaderboard> GetLeaderboardMappings() => new ColumnMappings<Leaderboard>("Leaderboards")
        {
            d => d.LeaderboardId,
            d => d.LastUpdate,
            d => d.DisplayName,
            d => d.IsProduction,
            d => d.ProductId,
            d => d.ModeId,
            d => d.RunId,
            d => d.CharacterId,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            IEnumerable<Leaderboard> leaderboards,
            CancellationToken cancellationToken = default)
        {
            var upserter = new TypedUpserter<Leaderboard>(GetLeaderboardMappings());

            return SaveChangesAsync(upserter, leaderboards, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Leaderboard> upserter,
            IEnumerable<Leaderboard> leaderboards,
            CancellationToken cancellationToken = default)
        {
            return upserter.UpsertAsync(connection, leaderboards, true, cancellationToken);
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
            IEnumerable<Entry> entries,
            CancellationToken cancellationToken = default)
        {
            var upserter = new TypedUpserter<Entry>(GetEntryMappings());

            return SaveChangesAsync(upserter, entries, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Entry> upserter,
            IEnumerable<Entry> entries,
            CancellationToken cancellationToken = default)
        {
            return upserter.InsertAsync(connection, entries, cancellationToken);
        }

        #endregion

        #region DailyLeaderboard

        public ColumnMappings<DailyLeaderboard> GetDailyLeaderboardMappings() => new ColumnMappings<DailyLeaderboard>("DailyLeaderboards")
        {
            d => d.LeaderboardId,
            d => d.LastUpdate,
            d => d.DisplayName,
            d => d.IsProduction,
            d => d.ProductId,
            d => d.Date,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            IEnumerable<DailyLeaderboard> leaderboards,
            CancellationToken cancellationToken = default)
        {
            var upserter = new TypedUpserter<DailyLeaderboard>(GetDailyLeaderboardMappings());

            return SaveChangesAsync(upserter, leaderboards, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<DailyLeaderboard> upserter,
            IEnumerable<DailyLeaderboard> leaderboards,
            CancellationToken cancellationToken = default)
        {
            return upserter.UpsertAsync(connection, leaderboards, true, cancellationToken);
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
            IEnumerable<DailyEntry> entries,
            CancellationToken cancellationToken = default)
        {
            var upserter = new TypedUpserter<DailyEntry>(GetDailyEntryMappings());

            return SaveChangesAsync(upserter, entries, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<DailyEntry> upserter,
            IEnumerable<DailyEntry> entries,
            CancellationToken cancellationToken = default)
        {
            return upserter.UpsertAsync(connection, entries, true, cancellationToken);
        }

        #endregion

        #region Player

        public ColumnMappings<Player> GetPlayerMappings() => new ColumnMappings<Player>("Players")
        {
            d => d.SteamId,
            d => d.LastUpdate,
            d => d.Exists,
            d => d.Name,
            d => d.Avatar,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            IEnumerable<Player> players,
            bool updateOnMatch,
            CancellationToken cancellationToken = default)
        {
            var upserter = new TypedUpserter<Player>(GetPlayerMappings());

            return SaveChangesAsync(upserter, players, updateOnMatch, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Player> upserter,
            IEnumerable<Player> players,
            bool updateOnMatch,
            CancellationToken cancellationToken = default)
        {
            return upserter.UpsertAsync(connection, players, updateOnMatch, cancellationToken);
        }

        #endregion

        #region Replay

        public ColumnMappings<Replay> GetReplayMappings() => new ColumnMappings<Replay>("Replays")
        {
            d => d.ReplayId,
            d => d.ErrorCode,
            d => d.Seed,
            d => d.Version,
            d => d.KilledBy,
            d => d.Uri,
        };

        [ExcludeFromCodeCoverage]
        public Task<int> SaveChangesAsync(
            IEnumerable<Replay> replays,
            bool updateOnMatch,
            CancellationToken cancellationToken = default)
        {
            var upserter = new TypedUpserter<Replay>(GetReplayMappings());

            return SaveChangesAsync(upserter, replays, updateOnMatch, cancellationToken);
        }

        internal Task<int> SaveChangesAsync(
            ITypedUpserter<Replay> upserter,
            IEnumerable<Replay> replays,
            bool updateOnMatch,
            CancellationToken cancellationToken = default)
        {
            return upserter.UpsertAsync(connection, replays, updateOnMatch, cancellationToken);
        }

        #endregion
    }
}