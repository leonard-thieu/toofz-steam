using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public interface IToofzApiClient : IDisposable
    {
        Task<PlayersEnvelope> GetPlayersAsync(
            GetPlayersParams @params = default,
            CancellationToken cancellationToken = default);
        Task<BulkStoreDTO> PostPlayersAsync(
            IEnumerable<Player> players,
            CancellationToken cancellationToken = default);
        Task<ReplaysEnvelope> GetReplaysAsync(
            GetReplaysParams @params = default,
            CancellationToken cancellationToken = default);
        Task<BulkStoreDTO> PostReplaysAsync(
            IEnumerable<Replay> replays,
            CancellationToken cancellationToken = default);
    }
}