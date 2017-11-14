using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using RichardSzalay.MockHttp;
using toofz.TestsShared;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class TransientFaultHandlerBaseTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new TransientFaultHandlerBaseAdapter();

                // Assert
                Assert.IsAssignableFrom<TransientFaultHandlerBaseAdapter>(handler);
            }
        }

        public class SendAsyncMethod
        {
            [Fact]
            public async Task ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler.When("*").Respond(new StringContent("0123456789"));
                var transientFaultHandler = new TransientFaultHandlerBaseAdapter() { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(transientFaultHandler);
                var request = new HttpRequestMessage();

                // Act
                var response = await handler.PublicSendAsync(request);

                // Assert
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }
        }

        private class TransientFaultHandlerBaseAdapter : TransientFaultHandlerBase
        {
            public TransientFaultHandlerBaseAdapter()
            {
                RetryPolicy = Policy.Handle<HttpRequestStatusException>().RetryAsync();
            }

            protected override RetryPolicy RetryPolicy { get; }
        }
    }
}
