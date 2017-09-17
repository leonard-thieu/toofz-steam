using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    class SteamWebApiTransientFaultHandlerTests
    {
        [TestClass]
        public class SendAsyncMethod
        {
            [TestMethod]
            public async Task ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(new StringContent("0123456789"));
                var handler = new HttpMessageHandlerAdapter(new SteamWebApiTransientFaultHandler { InnerHandler = mockHandler });
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
