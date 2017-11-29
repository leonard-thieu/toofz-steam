using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class GZipHandlerTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new GZipHandler();

                // Assert
                Assert.IsAssignableFrom<GZipHandler>(handler);
            }
        }

        public class SendAsyncMethod
        {
            public SendAsyncMethod()
            {
                var decompressionHandler = new GZipHandler { InnerHandler = mockHandler };
                handler = new HttpMessageHandlerAdapter(decompressionHandler);
            }

            private readonly MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            private readonly HttpMessageHandlerAdapter handler;

            [Fact]
            public async Task AddsAcceptEncodingGzipToRequest()
            {
                // Arrange
                mockHandler.When("*").Respond(HttpStatusCode.OK);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                var request = response.RequestMessage;
                Assert.Contains(new StringWithQualityHeaderValue("gzip"), request.Headers.AcceptEncoding.ToList());
            }

            [Fact]
            public async Task NoContent_ReturnsResponse()
            {
                // Arrange
                mockHandler.When("*").Respond(HttpStatusCode.OK);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }

            [Fact]
            public async Task ContentIsNotCompressed_ReturnsUnmodifiedResponse()
            {
                // Arrange
                var stream = new MemoryStream(Encoding.UTF8.GetBytes("0123456789"));
                var content = new StreamContent(stream);
                mockHandler.When("*").Respond(content);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                Assert.Same(content, response.Content);
            }

            [Fact]
            public async Task ContentIsCompressed_ReturnsResponseWithDecompressedContent()
            {
                // Arrange
                var compressed = new MemoryStream();
                using (var gzip = new GZipStream(compressed, CompressionMode.Compress, leaveOpen: true))
                {
                    gzip.Write(Encoding.UTF8.GetBytes("0123456789"), 0, 10);
                }
                compressed.Position = 0;
                var content = new StreamContent(compressed);
                content.Headers.ContentEncoding.Add("gzip");
                mockHandler.When("*").Respond(content);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                var content2 = await response.Content.ReadAsStringAsync();
                Assert.Equal("0123456789", content2);
            }
        }
    }
}
