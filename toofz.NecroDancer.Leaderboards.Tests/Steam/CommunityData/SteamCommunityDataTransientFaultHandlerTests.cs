using System;
using Microsoft.ApplicationInsights;
using toofz.NecroDancer.Leaderboards.Steam.CommunityData;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    public class SteamCommunityDataTransientFaultHandlerTests
    {
        public class Constructor
        {
            [Fact]
            public void TelemetryClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                TelemetryClient telemetryClient = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamCommunityDataTransientFaultHandler(telemetryClient);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var telemetryClient = new TelemetryClient();

                // Act
                var handler = new SteamCommunityDataTransientFaultHandler(telemetryClient);

                // Assert
                Assert.IsAssignableFrom<SteamCommunityDataTransientFaultHandler>(handler);
            }
        }
    }
}
