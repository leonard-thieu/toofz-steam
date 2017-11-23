using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Leaderboards.Steam
{
    public sealed class UgcHttpClient : IUgcHttpClient
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UgcHttpClient"/> class.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="telemetryClient">The telemetry client to use for reporting telemetry.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public UgcHttpClient(HttpMessageHandler handler, TelemetryClient telemetryClient) : this(handler, false, telemetryClient) { }

        /// <summary>
        /// Initializes an instance of the <see cref="UgcHttpClient"/> class.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="disposeHandler">
        /// true if the inner handler should be disposed of by <see cref="Dispose"/>,
        /// false if you intend to reuse the inner handler.
        /// </param>
        /// <param name="telemetryClient">The telemetry client to use for reporting telemetry.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        internal UgcHttpClient(HttpMessageHandler handler, bool disposeHandler, TelemetryClient telemetryClient)
        {
            http = new ProgressReporterHttpClient(handler, disposeHandler, telemetryClient);
        }

        private readonly ProgressReporterHttpClient http;

        #region GetUgcFile

        public async Task<byte[]> GetUgcFileAsync(
            string requestUri,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default)
        {
            var response = await http.GetAsync("Get UGC file", requestUri, progress, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        #endregion

        #region IDisposable Implementation

        private bool disposed;

        public void Dispose()
        {
            if (disposed) { return; }

            http.Dispose();

            disposed = true;
        }

        #endregion
    }
}
