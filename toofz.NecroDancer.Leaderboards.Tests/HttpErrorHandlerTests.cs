using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class HttpErrorHandlerTests
    {
        public class SendAsyncMethod
        {
            [Fact]
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
                await Assert.ThrowsAsync<HttpRequestStatusException>(() =>
                {
                    return handler.PublicSendAsync(request);
                });
            }

            [Fact]
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
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }
        }
    }
}
