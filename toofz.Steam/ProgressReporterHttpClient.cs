using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace toofz.Steam
{
    internal sealed class ProgressReporterHttpClient : HttpClient
    {
        /// <summary>
        /// Indicates if an exception is a transient fault for <see cref="ProgressReporterHttpClient"/>.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>
        /// true, if the exception is a transient fault for <see cref="ProgressReporterHttpClient"/>; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This checks exceptions that occur during an HTTP request (before the response is received). All clients that 
        /// use <see cref="ProgressReporterHttpClient"/> should include this check in their transient fault check.
        /// </remarks>
        public static bool IsTransient(Exception ex)
        {
            if (ex is HttpRequestException hre)
            {
                if (hre.InnerException is WebException we)
                {
                    // https://blogs.msdn.microsoft.com/jpsanders/2009/01/07/you-receive-one-or-more-error-messages-when-you-try-to-make-an-http-request-in-an-application-that-is-built-on-the-net-framework-2-0/
                    switch (we.Status)
                    {
                        case WebExceptionStatus.ConnectFailure:
                        case WebExceptionStatus.SendFailure:
                        case WebExceptionStatus.PipelineFailure:
                        case WebExceptionStatus.RequestCanceled:
                        case WebExceptionStatus.ConnectionClosed:
                        case WebExceptionStatus.KeepAliveFailure:
                        case WebExceptionStatus.UnknownError:
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ProgressReporterHttpClient"/> class.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="disposeHandler">
        /// true if the inner handler should be disposed of by <see cref="HttpMessageInvoker.Dispose()"/>,
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
                catch (Exception) when (operation.Telemetry.MarkAsUnsuccessful())
                {
                    // Unreachable
                    throw;
                }
            }
        }
    }
}
