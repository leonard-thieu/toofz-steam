using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class ContentLengthHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var httpContent = response.Content;
            if (httpContent != null)
            {
                await httpContent.LoadIntoBufferAsync().ConfigureAwait(false);

                var content = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                httpContent.Headers.ContentLength = content.Length;
            }

            return response;
        }
    }
}
