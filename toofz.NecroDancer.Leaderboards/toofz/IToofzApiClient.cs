using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public interface IToofzApiClient : IDisposable
    {
        Task<PlayersEnvelope> GetPlayersAsync(
            GetPlayersParams @params = null,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<BulkStoreDTO> PostPlayersAsync(
            IEnumerable<Player> players,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<ReplaysEnvelope> GetReplaysAsync(
            GetReplaysParams @params = null,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<BulkStoreDTO> PostReplaysAsync(
            IEnumerable<Replay> replays,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}