using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam
{
    public sealed class UgcHttpClient : IUgcHttpClient
    {
        public UgcHttpClient(HttpMessageHandler handler)
        {
            http = new ProgressReporterHttpClient(handler);
        }

        readonly ProgressReporterHttpClient http;

        #region GetUgcFile

        public async Task<byte[]> GetUgcFileAsync(
            string requestUri,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default)
        {
            var response = await http.GetAsync(requestUri, progress, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        #endregion

        #region IDisposable Implementation

        bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                http.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
