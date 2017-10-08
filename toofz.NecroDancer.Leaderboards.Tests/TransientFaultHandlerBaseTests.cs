using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class TransientFaultHandlerBaseTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void RetryPolicyIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                RetryPolicy retryPolicy = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new TransientFaultHandlerBaseAdapter(retryPolicy);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var errorDetectionStrategy = Mock.Of<ITransientErrorDetectionStrategy>();
                var retryPolicy = new RetryPolicy(errorDetectionStrategy, 1);

                // Act
                var handler = new TransientFaultHandlerBaseAdapter(retryPolicy);

                // Assert
                Assert.IsInstanceOfType(handler, typeof(TransientFaultHandlerBaseAdapter));
            }
        }

        [TestClass]
        public class SendAsyncMethod
        {
            [TestMethod]
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
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }
        }

        class TransientFaultHandlerBaseAdapter : TransientFaultHandlerBase
        {
            public TransientFaultHandlerBaseAdapter(RetryPolicy retryPolicy) : base(retryPolicy) { }
        }
    }
}
