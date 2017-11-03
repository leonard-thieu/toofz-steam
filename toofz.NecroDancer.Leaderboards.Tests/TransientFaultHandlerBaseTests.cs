using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moq;
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
            public void RetryPolicyIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                RetryPolicy retryPolicy = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new TransientFaultHandlerBaseAdapter(retryPolicy);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var errorDetectionStrategy = Mock.Of<ITransientErrorDetectionStrategy>();
                var retryPolicy = new RetryPolicy(errorDetectionStrategy, 1);

                // Act
                var handler = new TransientFaultHandlerBaseAdapter(retryPolicy);

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
                var errorDetectionStrategy = Mock.Of<ITransientErrorDetectionStrategy>();
                var retryPolicy = new RetryPolicy(errorDetectionStrategy, 1);
                var transientFaultHandler = new TransientFaultHandlerBaseAdapter(retryPolicy) { InnerHandler = mockHandler };
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
            public TransientFaultHandlerBaseAdapter(RetryPolicy retryPolicy) : base(retryPolicy) { }
        }
    }
}
