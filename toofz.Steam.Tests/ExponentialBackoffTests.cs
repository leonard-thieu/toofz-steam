using System;
using Xunit;

namespace toofz.Steam.Tests
{
    public class ExponentialBackoffTests
    {
        public class GetSleepDurationProviderMethod
        {
            [Fact]
            public void ReturnsSleepDurationProvider()
            {
                // Arrange
                var minBackoff = TimeSpan.FromSeconds(1);
                var maxBackoff = TimeSpan.FromSeconds(10);
                var deltaBackoff = TimeSpan.FromSeconds(2);

                // Act
                var backoff = ExponentialBackoff.GetSleepDurationProvider(minBackoff, maxBackoff, deltaBackoff);

                // Assert
                Assert.IsAssignableFrom<Func<int, TimeSpan>>(backoff);
            }

            [Fact]
            public void SleepDurationProvider_ReturnsSleepDuration()
            {
                // Arrange
                var minBackoff = TimeSpan.FromSeconds(1);
                var maxBackoff = TimeSpan.FromSeconds(10);
                var deltaBackoff = TimeSpan.FromSeconds(2);
                var backoff = ExponentialBackoff.GetSleepDurationProvider(minBackoff, maxBackoff, deltaBackoff);
                var currentRetryCount = 1;

                // Act
                var sleep = backoff(currentRetryCount);

                // Assert
                Assert.InRange(sleep, minBackoff, maxBackoff);
            }
        }

        public class GetSleepDurationMethod
        {
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
                var sleep = ExponentialBackoff.GetSleepDuration(currentRetryCount, minBackoff, maxBackoff, deltaBackoff, jitterer);

                // Assert
                Assert.Equal(31800000, sleep.Ticks);
            }
        }
    }
}
