using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public interface IToofzApiClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the base address of Uniform Resource Identifier (URI) of the Internet 
        /// resource used when sending requests.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Uri"/>.The base address of Uniform Resource Identifier (URI) of the 
        /// Internet resource used when sending requests.
        /// </returns>
        Uri BaseAddress { get; set; }
        Task<PlayersEnvelope> GetPlayersAsync(
            GetPlayersParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        Task<BulkStoreDTO> PostPlayersAsync(
            IEnumerable<Player> players,
            CancellationToken cancellationToken = default);
        Task<ReplaysEnvelope> GetReplaysAsync(
            GetReplaysParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
        Task<BulkStoreDTO> PostReplaysAsync(
            IEnumerable<Replay> replays,
            CancellationToken cancellationToken = default);
    }
}