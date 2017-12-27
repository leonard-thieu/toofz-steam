using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.Steam.Tests
{
    public class TransientFaultHandlerTests
    {
        public class Constructor
        {
            [DisplayFact(nameof(ArgumentNullException))]
            public void PolicyIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                Policy policy = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new TransientFaultHandler(policy);
                });
            }

            [DisplayFact(nameof(TransientFaultHandler))]
            public void ReturnsTransientFaultHandler()
            {
                // Arrange
                var policy = Policy.NoOpAsync();

                // Act
                var handler = new TransientFaultHandler(policy);

                // Assert
                Assert.IsAssignableFrom<TransientFaultHandler>(handler);
            }
        }

        public class SendAsyncMethod
        {
            public SendAsyncMethod()
            {
                transientFaultHandler = new TransientFaultHandler(policy) { InnerHandler = mockHandler };
                handler = new HttpMessageHandlerAdapter(transientFaultHandler);
            }

            private readonly MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
            private readonly Policy policy = Policy.NoOpAsync();
            private readonly TransientFaultHandler transientFaultHandler;
            private readonly HttpMessageHandlerAdapter handler;

            [DisplayFact]
            public async Task ReturnsResponse()
            {
                // Arrange
                mockHandler.When("*").Respond(new StringContent("0123456789"));
                var request = new HttpRequestMessage();

                // Act
                var response = await handler.PublicSendAsync(request);

                // Assert
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }
        }
    }
}
