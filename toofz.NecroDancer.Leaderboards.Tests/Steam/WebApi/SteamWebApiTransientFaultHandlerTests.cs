using System;
using Microsoft.ApplicationInsights;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    public class SteamWebApiTransientFaultHandlerTests
    {
        public class Costructor
        {
            [Fact]
            public void TelemetryClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                TelemetryClient telemetryClient = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamWebApiTransientFaultHandler(telemetryClient);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var telemetryClient = new TelemetryClient();

                // Act
                var handler = new SteamWebApiTransientFaultHandler(telemetryClient);

                // Assert
                Assert.IsAssignableFrom<SteamWebApiTransientFaultHandler>(handler);
            }
        }
    }
}
