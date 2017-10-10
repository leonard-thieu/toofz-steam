using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class GZipHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            var requestTask = base.SendAsync(request, cancellationToken);

            var response = await requestTask.ConfigureAwait(false);
            var httpContent = response.Content;
            if (httpContent != null &&
                httpContent.Headers.ContentEncoding.Contains("gzip"))
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
