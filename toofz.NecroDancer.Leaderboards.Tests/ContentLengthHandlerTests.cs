using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ContentLengthHandlerTests
    {
        [TestClass]
        public class SendAsyncMethod
        {
            [TestMethod]
            public async Task NoContent_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler.When("*").Respond(HttpStatusCode.OK);
                var decompressionHandler = new ContentLengthHandler { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(decompressionHandler);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }

            [TestMethod]
            public async Task SetsContentLengthToLengthOfContent()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(new StringContent("0123456789"));
                var handler = new HttpMessageHandlerAdapter(new ContentLengthHandler { InnerHandler = mockHandler });

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"));

                // Assert
                Assert.AreEqual(10, response.Content.Headers.ContentLength.Value);
            }
        }
    }
}
