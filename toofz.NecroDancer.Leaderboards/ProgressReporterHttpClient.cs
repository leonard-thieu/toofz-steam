using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ProgressReporterHttpClient : HttpClient
    {
        public ProgressReporterHttpClient(HttpMessageHandler handler) : base(handler) { }

        public ProgressReporterHttpClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) { }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            var response = await GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
            progress?.Report(response.Content.Headers.ContentLength.Value);

            return response;
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            return GetAsync(requestUri.ToString(), progress, cancellationToken);
        }
    }
}
