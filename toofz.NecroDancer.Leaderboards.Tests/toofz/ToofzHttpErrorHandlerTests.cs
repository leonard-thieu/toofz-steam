using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class ToofzHttpErrorHandlerTests
    {
        [TestClass]
        public class SendAsyncMethod
        {
            [TestMethod]
            public async Task RequestIsNotSuccessfulAndHttpErrorIsReturned_ThrowsHttpErrorException()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.BadRequest, "application/json", JsonConvert.SerializeObject(new HttpError("myMessage")));
                var handler = new HttpMessageHandlerAdapter(new ToofzHttpErrorHandler { InnerHandler = mockHandler });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<HttpErrorException>(() =>
                {
                    return handler.PublicSendAsync(request);
                });
            }

            [TestMethod]
            public async Task RequestIsNotSuccessfulAndHttpErrorIsNotReturned_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.BadRequest, "application/json", JsonConvert.SerializeObject(new { myProp = "myProp" }));
                var handler = new HttpMessageHandlerAdapter(new ToofzHttpErrorHandler { InnerHandler = mockHandler });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;

                // Act
                var response = await handler.PublicSendAsync(request);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }

            [TestMethod]
            public async Task ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.OK);
                var handler = new HttpMessageHandlerAdapter(new ToofzHttpErrorHandler { InnerHandler = mockHandler });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;

                // Act
                var response = await handler.PublicSendAsync(request);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }
        }
    }
}
