using System;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class RetryUtilTests
    {
        public class GetExponentialBackoffMethod
        {
            #region Func<int, TimeSpan> GetExponentialBackoff(TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)

            [Fact]
            public void ReturnsExponentialBackoffSleepDurationProvider()
            {
                // Arrange
                var minBackoff = TimeSpan.FromSeconds(1);
                var maxBackoff = TimeSpan.FromSeconds(10);
                var deltaBackoff = TimeSpan.FromSeconds(2);

                // Act
                var backoff = RetryUtil.GetExponentialBackoff(minBackoff, maxBackoff, deltaBackoff);

                // Assert
                Assert.IsAssignableFrom<Func<int, TimeSpan>>(backoff);
            }

            [Fact]
            public void SleepDurationProviderReturnsSleepDuration()
            {
                // Arrange
                var minBackoff = TimeSpan.FromSeconds(1);
                var maxBackoff = TimeSpan.FromSeconds(10);
                var deltaBackoff = TimeSpan.FromSeconds(2);
                var backoff = RetryUtil.GetExponentialBackoff(minBackoff, maxBackoff, deltaBackoff);
                var currentRetryCount = 1;

                // Act
                var sleep = backoff(currentRetryCount);

                // Assert
                Assert.InRange(sleep, minBackoff, maxBackoff);
            }

            #endregion

            #region TimeSpan GetExponentialBackoff(int currentRetryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff, Random jitterer)

            [Fact]
            public void ReturnsSleepDuration()
            {
                // Arrange
                var currentRetryCount = 1;
                var minBackoff = TimeSpan.FromSeconds(1);
                var maxBackoff = TimeSpan.FromSeconds(10);
                var deltaBackoff = TimeSpan.FromSeconds(2);
                var jitterer = new Random(0);

                // Act
                var sleep = RetryUtil.GetExponentialBackoff(currentRetryCount, minBackoff, maxBackoff, deltaBackoff, jitterer);

                // Assert
                Assert.Equal(31800000, sleep.Ticks);
            }

            #endregion
        }
    }
}
