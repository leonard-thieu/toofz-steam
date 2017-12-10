using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Xunit;

namespace toofz.Steam.Tests
{
    public class OperationTelemetryExtensionsTests
    {
        public class MarKAsUnsuccessfulMethod
        {
            [DisplayFact(nameof(OperationTelemetry.Success))]
            public void SetsSuccessToFalse()
            {
                // Arrange
                var telemetry = new DependencyTelemetry();

                // Act
                telemetry.MarkAsUnsuccessful();

                // Assert
                Assert.False(telemetry.Success);
            }

            [DisplayFact]
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
