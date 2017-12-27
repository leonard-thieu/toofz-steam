using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace toofz.Steam.Workshop
{
    /// <summary>
    /// HTTP client used for downloading user-generated content (UGC) from Steam Workshop.
    /// </summary>
    public sealed class UgcHttpClient : IUgcHttpClient
    {
        /// <summary>
        /// Indicates if an exception is a transient fault for <see cref="UgcHttpClient"/>.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>
        /// true, if the exception is a transient fault for <see cref="UgcHttpClient"/>; otherwise, false.
        /// </returns>
        public static bool IsTransient(Exception ex)
        {
            return ProgressReporterHttpClient.IsTransient(ex);
        }

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

        /// <summary>
        /// Gets a UGC file as binary data.
        /// </summary>
        /// <param name="requestUri">The URI to download the UGC file from.</param>
        /// <param name="progress">An optional <see cref="IProgress{T}"/> object used to report download size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// The UGC file as binary data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestUri"/> is null.
        /// </exception>
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

        /// <summary>
        /// Disposes of the resources used by <see cref="UgcHttpClient"/>.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }

            http.Dispose();

            disposed = true;
        }

        #endregion
    }
}
