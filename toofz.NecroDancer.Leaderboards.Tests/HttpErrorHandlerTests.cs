using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpErrorHandlerTests
    {
        [TestClass]
        public class SendAsyncMethod
        {
            [TestMethod]
            public async Task RequestIsNotSuccessful_ThrowsHttpRequestStatusException()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.BadRequest, new StringContent("myContent"));
                var handler = new TestingHttpMessageHandler(new HttpErrorHandler { InnerHandler = mockHandler });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<HttpRequestStatusException>(() =>
                {
                    return handler.TestSendAsync(request);
                });
            }

            [TestMethod]
            public async Task ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.OK);
                var handler = new TestingHttpMessageHandler(new HttpErrorHandler { InnerHandler = mockHandler });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;

                // Act
                var response = await handler.TestSendAsync(request);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }
        }
    }
}
