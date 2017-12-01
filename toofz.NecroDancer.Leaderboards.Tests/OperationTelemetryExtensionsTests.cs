using Microsoft.ApplicationInsights.DataContracts;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class OperationTelemetryExtensionsTests
    {
        public class MarKAsUnsuccessfulMethod
        {
            [Fact]
            public void SetsSuccessToFalse()
            {
                // Arrange
                var telemetry = new DependencyTelemetry();

                // Act
                telemetry.MarkAsUnsuccessful();

                // Assert
                Assert.False(telemetry.Success);
            }

            [Fact]
            public void ReturnsFalse()
            {
                // Arrange
                var telemetry = new DependencyTelemetry();

                // Act
                var shouldHandle = telemetry.MarkAsUnsuccessful();

                // Assert
                Assert.False(shouldHandle);
            }
        }
    }
}
