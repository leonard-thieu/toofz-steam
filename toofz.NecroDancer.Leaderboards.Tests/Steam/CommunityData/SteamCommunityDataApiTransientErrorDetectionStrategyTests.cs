using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Leaderboards.Steam.CommunityData;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    class SteamCommunityDataApiTransientErrorDetectionStrategyTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var strategy = new SteamCommunityDataApiTransientErrorDetectionStrategy();

                // Assert
                Assert.IsInstanceOfType(strategy, typeof(SteamCommunityDataApiTransientErrorDetectionStrategy));
            }
        }
        [TestClass]
        public class IsTransientMethod
        {
            [TestMethod]
            public void ExIsNotHttpRequestStatusException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamCommunityDataApiTransientErrorDetectionStrategy();
                var ex = new Exception();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsFalse(isTransient);
            }

            [DataTestMethod]
            [DataRow(HttpStatusCode.Forbidden)]
            [DataRow(HttpStatusCode.RequestTimeout)]
            [DataRow(HttpStatusCode.InternalServerError)]
            [DataRow(HttpStatusCode.BadGateway)]
            [DataRow(HttpStatusCode.ServiceUnavailable)]
            [DataRow(HttpStatusCode.GatewayTimeout)]
            public void ExIsHttpRequestStatusExceptionAndStatusCodeIsTransient_ReturnsTrue(HttpStatusCode statusCode)
            {
                // Arrange
                var strategy = new SteamCommunityDataApiTransientErrorDetectionStrategy();
                var ex = new HttpRequestStatusException(statusCode, new Uri("http://localhost/"));

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsTrue(isTransient);
            }

            [TestMethod]
            public void ExIsHttpRequestStatusExceptionAndStatusCodeIsNotTransient_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamCommunityDataApiTransientErrorDetectionStrategy();
                var ex = new HttpRequestStatusException(HttpStatusCode.NotFound, new Uri("http://localhost/"));

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsFalse(isTransient);
            }
        }
    }
}
