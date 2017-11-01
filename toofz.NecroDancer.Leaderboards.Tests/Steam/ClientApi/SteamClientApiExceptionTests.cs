using System;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientApiExceptionTests
    {
        public class Constructor_String
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";

                // Act
                var ex = new SteamClientApiException(message);

                // Assert
                Assert.IsAssignableFrom<SteamClientApiException>(ex);
            }
        }


        public class Constructor_String_Exception
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var inner = new Exception();

                // Act
                var ex = new SteamClientApiException(message, inner);

                // Assert
                Assert.IsAssignableFrom<SteamClientApiException>(ex);
            }
        }


        public class Constructor_String_EResult
        {
            [Fact]
            public void SetsResult()
            {
                // Arrange
                var message = "myMessage";
                var result = EResult.AccessDenied;

                // Act
                var ex = new SteamClientApiException(message, result);

                // Assert
                Assert.Equal(result, ex.Result);
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var result = EResult.AccessDenied;

                // Act
                var ex = new SteamClientApiException(message, result);

                // Assert
                Assert.IsAssignableFrom<SteamClientApiException>(ex);
            }
        }
    }
}
