using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpErrorHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var message = $"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).";

                await response.Content.LoadIntoBufferAsync().ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                throw new HttpRequestStatusException(message, response.StatusCode, response.RequestMessage.RequestUri, responseContent);
            }

            return response;
        }
    }
}
