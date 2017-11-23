using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ProgressReporterHttpClient : HttpClient
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ProgressReporterHttpClient"/> class.
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
        public ProgressReporterHttpClient(HttpMessageHandler handler, bool disposeHandler, TelemetryClient telemetryClient) : base(handler, disposeHandler)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        private readonly TelemetryClient telemetryClient;

        public async Task<HttpResponseMessage> GetAsync(
            string operationName,
            string requestUri,
            IProgress<long> progress,
            CancellationToken cancellationToken)
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            using (var operation = telemetryClient.StartOperation<DependencyTelemetry>(operationName))
            {
                operation.Telemetry.Type = "Http";
                operation.Telemetry.Data = requestUri;

                try
                {
                    var response = await GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                    progress?.Report(response.Content.Headers.ContentLength.Value);

                    operation.Telemetry.Success = true;

                    return response;
                }
                catch (Exception)
                {
                    operation.Telemetry.Success = false;
                    throw;
                }
            }
        }
    }
}
