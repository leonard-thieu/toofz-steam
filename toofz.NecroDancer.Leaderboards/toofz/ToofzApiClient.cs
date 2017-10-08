using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using log4net;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class ToofzApiClient : HttpClient, IToofzApiClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(ToofzApiClient));

        /// <summary>
        /// Initializes a new instance of the <see cref="ToofzApiClient"/> class with a specific handler.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="disposeHandler">
        /// true if the inner handler should be disposed of by Dispose(); false if you intend 
        /// to reuse the inner handler.
        /// </param>
        public ToofzApiClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) { }

        #region Players

        public async Task<PlayersEnvelope> GetPlayersAsync(
            GetPlayersParams @params = null,
            CancellationToken cancellationToken = default)
        {
            var requestUri = "players";
            @params = @params ?? new GetPlayersParams();
            requestUri = requestUri.SetQueryParams(new
            {
                q = @params.Query,
                offset = @params.Offset,
                limit = @params.Limit,
                sort = @params.Sort,
            });

            var response = await GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<PlayersEnvelope>().ConfigureAwait(false);
        }

        public async Task<BulkStoreDTO> PostPlayersAsync(
            IEnumerable<Player> players,
            CancellationToken cancellationToken = default)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));

            var response = await this.PostAsJsonAsync("players", players, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<BulkStoreDTO>().ConfigureAwait(false);
        }

        #endregion

        #region Replays

        public async Task<ReplaysEnvelope> GetReplaysAsync(
            GetReplaysParams @params = null,
            CancellationToken cancellationToken = default)
        {
            var requestUri = "replays";
            @params = @params ?? new GetReplaysParams();
            requestUri = requestUri.SetQueryParams(new
            {
                version = @params.Version,
                error = @params.ErrorCode,
                offset = @params.Offset,
                limit = @params.Limit,
            });

            var response = await GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<ReplaysEnvelope>().ConfigureAwait(false);
        }

        public async Task<BulkStoreDTO> PostReplaysAsync(
            IEnumerable<Replay> replays,
            CancellationToken cancellationToken = default)
        {
            if (replays == null)
                throw new ArgumentNullException(nameof(replays));

            var response = await this.PostAsJsonAsync("replays", replays, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<BulkStoreDTO>().ConfigureAwait(false);
        }

        #endregion
    }
}