using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            public async Task SetsContentLengthToLengthOfContent()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(new StringContent("0123456789"));
                var handler = new TestingHttpMessageHandler(new ContentLengthHandler { InnerHandler = mockHandler });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;

                // Act
                var response = await handler.TestSendAsync(request);

                // Assert
                Assert.AreEqual(10, response.Content.Headers.ContentLength.Value);
            }
        }
    }
}
