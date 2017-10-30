using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class HttpErrorHandlerTests
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
                var handler = new HttpMessageHandlerAdapter(new HttpErrorHandler { InnerHandler = mockHandler });
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<HttpRequestStatusException>(() =>
                {
                    return handler.PublicSendAsync(request);
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
                var handler = new HttpMessageHandlerAdapter(new HttpErrorHandler { InnerHandler = mockHandler });
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");

                // Act
                var response = await handler.PublicSendAsync(request);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }
        }
    }
}
