using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class GZipHandlerTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new GZipHandler();

                // Assert
                Assert.IsInstanceOfType(handler, typeof(GZipHandler));
            }
        }

        [TestClass]
        public class SendAsyncMethod
        {
            public SendAsyncMethod()
            {
                mockHandler = new MockHttpMessageHandler();
                var decompressionHandler = new GZipHandler { InnerHandler = mockHandler };
                handler = new HttpMessageHandlerAdapter(decompressionHandler);
            }

            MockHttpMessageHandler mockHandler;
            HttpMessageHandlerAdapter handler;

            [TestMethod]
            public async Task AddsAcceptEncodingGzipToRequest()
            {
                // Arrange
                mockHandler.When("*").Respond(HttpStatusCode.OK);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                var request = response.RequestMessage;
                CollectionAssert.Contains(request.Headers.AcceptEncoding.ToList(), new StringWithQualityHeaderValue("gzip"));
            }

            [TestMethod]
            public async Task NoContent_ReturnsResponse()
            {
                // Arrange
                mockHandler.When("*").Respond(HttpStatusCode.OK);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }

            [TestMethod]
            public async Task ContentIsNotCompressed_ReturnsUnmodifiedResponse()
            {
                // Arrange
                var stream = new MemoryStream(Encoding.UTF8.GetBytes("0123456789"));
                var content = new StreamContent(stream);
                mockHandler.When("*").Respond(content);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                Assert.AreSame(content, response.Content);
            }

            [TestMethod]
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
                Assert.AreEqual("0123456789", content2);
            }
        }
    }
}
