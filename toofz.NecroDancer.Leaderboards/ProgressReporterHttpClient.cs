using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ProgressReporterHttpClient : HttpClient
    {
        private static readonly TelemetryClient TelemetryClient = new TelemetryClient();

        public ProgressReporterHttpClient(HttpMessageHandler handler) : base(handler) { }

        public ProgressReporterHttpClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) { }

        public async Task<HttpResponseMessage> GetAsync(
            string requestUri,
            IProgress<long> progress,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "")
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            using (var operation = TelemetryClient.StartOperation<DependencyTelemetry>(memberName))
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
                catch (Exception ex)
                {
                    TelemetryClient.TrackException(ex);
                    operation.Telemetry.Success = false;
                    throw;
                }
            }
        }
    }
}
