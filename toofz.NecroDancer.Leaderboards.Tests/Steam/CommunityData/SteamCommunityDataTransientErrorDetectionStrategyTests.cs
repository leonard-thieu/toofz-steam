using System;
using System.Net;
using toofz.NecroDancer.Leaderboards.Steam.CommunityData;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    public class SteamCommunityDataTransientErrorDetectionStrategyTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var strategy = new SteamCommunityDataTransientErrorDetectionStrategy();

                // Assert
                Assert.IsAssignableFrom<SteamCommunityDataTransientErrorDetectionStrategy>(strategy);
            }
        }

        public class IsTransientMethod
        {
            [Fact]
            public void ExIsNotHttpRequestStatusException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamCommunityDataTransientErrorDetectionStrategy();
                var ex = new Exception();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }

            [Theory]
            [InlineData(HttpStatusCode.Forbidden)]
            [InlineData(HttpStatusCode.RequestTimeout)]
            [InlineData(HttpStatusCode.InternalServerError)]
            [InlineData(HttpStatusCode.BadGateway)]
            [InlineData(HttpStatusCode.ServiceUnavailable)]
            [InlineData(HttpStatusCode.GatewayTimeout)]
            public void ExIsHttpRequestStatusExceptionAndStatusCodeIsTransient_ReturnsTrue(HttpStatusCode statusCode)
            {
                // Arrange
                var strategy = new SteamCommunityDataTransientErrorDetectionStrategy();
                var ex = new HttpRequestStatusException(statusCode, new Uri("http://localhost/"));

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [Fact]
            public void ExIsHttpRequestStatusExceptionAndStatusCodeIsNotTransient_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamCommunityDataTransientErrorDetectionStrategy();
                var ex = new HttpRequestStatusException(HttpStatusCode.NotFound, new Uri("http://localhost/"));

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }
        }
    }
}
