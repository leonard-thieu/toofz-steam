using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moq;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientApiTransientErrorDetectionStrategyTests
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
                var retryPolicy = SteamClientApiTransientErrorDetectionStrategy.CreateRetryPolicy(retryStrategy, log);

                // Assert
                Assert.IsAssignableFrom<RetryPolicy<SteamClientApiTransientErrorDetectionStrategy>>(retryPolicy);
            }

            [Fact]
            public void OnRetrying_LogsDebugMessage()
            {
                // Arrange
                var retryStrategy = new FixedInterval(1, TimeSpan.Zero);
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var retryPolicy = SteamClientApiTransientErrorDetectionStrategy.CreateRetryPolicy(retryStrategy, log);
                var count = 0;

                // Act
                retryPolicy.ExecuteAction(() =>
                {
                    if (count == 0)
                    {
                        count++;
                        throw new SteamClientApiException("", new TaskCanceledException());
                    }
                });

                // Assert
                mockLog.Verify(l => l.Debug(It.IsAny<string>()), Times.Once);
            }
        }


        public class IsTransientMethod
        {
            [Fact]
            public void ExIsSteamClientApiExceptionWithTaskCanceledExceptionAsInnerException_ReturnsTrue()
            {
                // Arrange
                var strategy = new SteamClientApiTransientErrorDetectionStrategy();
                var ex = new SteamClientApiException("myMessage", new TaskCanceledException());

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [Fact]
            public void ExIsSteamClientApiExceptionWithoutTaskCanceledExceptionAsInnerException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamClientApiTransientErrorDetectionStrategy();
                var ex = new SteamClientApiException("myMessage");

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }

            [Fact]
            public void ExIsNotTaskCanceledException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamClientApiTransientErrorDetectionStrategy();
                var ex = new Exception();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }
        }
    }
}
