using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    class SteamClientApiExceptionTests
    {
        [TestClass]
        public class Constructor_String
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";

                // Act
                var ex = new SteamClientApiException(message);

                // Assert
                Assert.IsInstanceOfType(ex, typeof(SteamClientApiException));
            }
        }

        [TestClass]
        public class Constructor_String_Exception
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var inner = new Exception();

                // Act
                var ex = new SteamClientApiException(message, inner);

                // Assert
                Assert.IsInstanceOfType(ex, typeof(SteamClientApiException));
            }
        }

        [TestClass]
        public class Constructor_String_EResult
        {
            [TestMethod]
            public void SetsResult()
            {
                // Arrange
                var message = "myMessage";
                var result = EResult.AccessDenied;

                // Act
                var ex = new SteamClientApiException(message, result);

                // Assert
                Assert.AreEqual(result, ex.Result);
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var result = EResult.AccessDenied;

                // Act
                var ex = new SteamClientApiException(message, result);

                // Assert
                Assert.IsInstanceOfType(ex, typeof(SteamClientApiException));
            }
        }
    }
}
