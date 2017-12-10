using System;
using SteamKit2;
using toofz.Steam.ClientApi;
using Xunit;

namespace toofz.Steam.Tests.ClientApi
{
    public class SteamClientApiExceptionTests
    {
        public class Constructor_String
        {
            [DisplayFact(nameof(SteamClientApiException))]
            public void ReturnsSteamClientApiException()
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
            [DisplayFact(nameof(SteamClientApiException))]
            public void ReturnsSteamClientApiException()
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
            [DisplayFact(nameof(SteamClientApiException.Result))]
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

            [DisplayFact(nameof(SteamClientApiException))]
            public void ReturnsSteamClientApiException()
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
