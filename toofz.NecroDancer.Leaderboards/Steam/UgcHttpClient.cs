using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam
{
    public sealed class UgcHttpClient : ProgressReporterHttpClient, IUgcHttpClient
    {
        #region Initialization

        public UgcHttpClient(HttpMessageHandler handler) : base(handler) { }

        #endregion

        #region GetUgcFile

        public async Task<byte[]> GetUgcFileAsync(
            string requestUri,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await GetAsync(requestUri, progress, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
