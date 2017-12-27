using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.Steam
{
    /// <summary>
    /// Enables and handles gzip decompression of HTTP streams.
    /// </summary>
    /// <remarks>
    /// This functionality is available through <see cref="HttpClientHandler.AutomaticDecompression"/>; however, 
    /// it causes <see cref="HttpContentHeaders.ContentLength"/> to report the decompressed size as the stream is decompressed 
    /// before <see cref="HttpContentHeaders.ContentLength"/> is set. <see cref="GZipHandler"/> allows <see cref="HttpContentHeaders.ContentLength"/> 
    /// to be set to the compressed size by enabling decompression to take place after <see cref="HttpContentHeaders.ContentLength"/> is set.
    /// </remarks>
    public sealed class GZipHandler : DelegatingHandler
    {
        private const string GZip = "gzip";

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="request"/> is null.
        /// </exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(GZip));
            var requestTask = base.SendAsync(request, cancellationToken);

            var response = await requestTask.ConfigureAwait(false);
            var httpContent = response.Content;
            if (httpContent != null &&
                httpContent.Headers.ContentEncoding.Contains(GZip))
            {
                var content = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                using (var gzip = new GZipStream(content, CompressionMode.Decompress, leaveOpen: true))
                {
                    var ms = new MemoryStream();
                    await gzip.CopyToAsync(ms, 81920, cancellationToken).ConfigureAwait(false);
                    ms.Position = 0;

                    response.Content = httpContent.Clone(ms);
                }
                httpContent.Dispose();
            }

            return response;
        }
    }
}
