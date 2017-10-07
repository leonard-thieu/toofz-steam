using System.IO;
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
            var content = response.Content;

            if (content.Headers.ContentEncoding.Contains("gzip"))
            {
                var inStream = await content.ReadAsStreamAsync().ConfigureAwait(false);
                using (var gzip = new GZipStream(inStream, CompressionMode.Decompress))
                {
                    var outStream = new MemoryStream();
                    await gzip.CopyToAsync(outStream, 1024, cancellationToken).ConfigureAwait(false);
                    response.Content = new StreamContent(outStream);
                }
                content.Dispose();
            }

            return response;
        }
    }
}
