using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class DecompressionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var httpContent = response.Content;
            if (httpContent != null && httpContent.Headers.ContentEncoding.Contains("gzip"))
            {
                var content = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                using (var gzip = new GZipStream(content, CompressionMode.Decompress, leaveOpen: true))
                {
                    response.Content = await httpContent.CloneAsync(gzip, cancellationToken).ConfigureAwait(false);
                }
                httpContent.Dispose();
            }

            return response;
        }
    }
}
