using System;
using System.Net;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moq;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    public class SteamWebApiTransientErrorDetectionStrategyTests
    {
        public class CreateRetryPolicyMethod
        {
            [Fact]
            public void ReturnsRetryPolicy()
            {
                // Arrange
                var retryStrategy = RetryStrategy.NoRetry;
                var log = Mock.Of<ILog>();

                // Act
                var retryPolicy = SteamWebApiTransientErrorDetectionStrategy.CreateRetryPolicy(retryStrategy, log);

                // Assert
                Assert.IsAssignableFrom<RetryPolicy<SteamWebApiTransientErrorDetectionStrategy>>(retryPolicy);
            }

            [Fact]
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


        public class IsTransientMethod
        {
            [Theory]
            [InlineData(HttpStatusCode.RequestTimeout)]
            [InlineData(HttpStatusCode.InternalServerError)]
            [InlineData(HttpStatusCode.BadGateway)]
            [InlineData(HttpStatusCode.ServiceUnavailable)]
            [InlineData(HttpStatusCode.GatewayTimeout)]
            public void ExIsHttpRequestStatusExceptionAndHasTransientStatusCode_ReturnsTrue(HttpStatusCode statusCode)
            {
                // Arrange
                var strategy = new SteamWebApiTransientErrorDetectionStrategy();
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [Fact]
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
                Assert.False(isTransient);
            }

            [Fact]
            public void ExIsNotHttpRequestStatusException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamWebApiTransientErrorDetectionStrategy();
                var ex = new Exception();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }
        }
    }
}
