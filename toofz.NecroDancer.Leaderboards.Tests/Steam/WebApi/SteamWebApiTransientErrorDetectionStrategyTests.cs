using System;
using System.Net;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    class SteamWebApiTransientErrorDetectionStrategyTests
    {
        [TestClass]
        public class CreateRetryPolicyMethod
        {
            [TestMethod]
            public void ReturnsRetryPolicy()
            {
                // Arrange
                var retryStrategy = RetryStrategy.NoRetry;
                var log = Mock.Of<ILog>();

                // Act
                var retryPolicy = SteamWebApiTransientErrorDetectionStrategy.CreateRetryPolicy(retryStrategy, log);

                // Assert
                Assert.IsInstanceOfType(retryPolicy, typeof(RetryPolicy<SteamWebApiTransientErrorDetectionStrategy>));
            }

            [TestMethod]
            public void OnRetrying_LogsDebugMessage()
            {
                // Arrange
                var retryStrategy = new FixedInterval(1, TimeSpan.Zero);
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var retryPolicy = SteamWebApiTransientErrorDetectionStrategy.CreateRetryPolicy(retryStrategy, log);
                var count = 0;

                // Act
                retryPolicy.ExecuteAction(() =>
                {
                    if (count == 0)
                    {
                        count++;
                        var statusCode = HttpStatusCode.BadGateway;
                        var requestUri = new Uri("http://localhost/");
                        throw new HttpRequestStatusException(statusCode, requestUri);
                    }
                });

                // Assert
                mockLog.Verify(l => l.Debug(It.IsAny<string>()), Times.Once);
            }
        }

        [TestClass]
        public class IsTransientMethod
        {
            [DataTestMethod]
            [DataRow(HttpStatusCode.RequestTimeout)]
            [DataRow(HttpStatusCode.InternalServerError)]
            [DataRow(HttpStatusCode.BadGateway)]
            [DataRow(HttpStatusCode.ServiceUnavailable)]
            [DataRow(HttpStatusCode.GatewayTimeout)]
            public void ExIsHttpRequestStatusExceptionAndHasTransientStatusCode_ReturnsTrue(HttpStatusCode statusCode)
            {
                // Arrange
                var strategy = new SteamWebApiTransientErrorDetectionStrategy();
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsTrue(isTransient);
            }

            [TestMethod]
            public void ExIsHttpRequestStatusExceptionAndDoesNotHaveTransientStatusCode_ReturnsFalse()
            {
                // Arrange
                var statusCode = HttpStatusCode.NotFound;
                var strategy = new SteamWebApiTransientErrorDetectionStrategy();
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsFalse(isTransient);
            }

            [TestMethod]
            public void ExIsNotHttpRequestStatusException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamWebApiTransientErrorDetectionStrategy();
                var ex = new Exception();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsFalse(isTransient);
            }
        }
    }
}
