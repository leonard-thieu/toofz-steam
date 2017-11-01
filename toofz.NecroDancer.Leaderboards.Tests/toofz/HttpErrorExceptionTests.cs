using System;
using System.Net;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class HttpErrorExceptionTests
    {
        public class Constructor
        {
            [Fact]
            public void HttpErrorIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpError httpError = null;
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");

                // Act -> Assert
                var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    new HttpErrorException(httpError, statusCode, requestUri);
                });
                Assert.Equal("httpError", ex.ParamName);
            }

            [Fact]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var httpError = new HttpError();
                var statusCode = HttpStatusCode.NotFound;
                Uri requestUri = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new HttpErrorException(httpError, statusCode, requestUri);
                });
            }

            [Fact]
            public void SetsMessage()
            {
                // Arrange
                var httpError = new HttpError { Message = "myMessage" };
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Act
                var message = ex.Message;

                // Assert
                Assert.Equal("myMessage", message);
            }

            [Fact]
            public void SetsStackTrace()
            {
                // Arrange
                var httpError = new HttpError { StackTrace = "myStackTrace" };
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Act
                var stackTrace = ex.StackTrace;

                // Assert
                Assert.Equal("myStackTrace", stackTrace);
            }

            [Fact]
            public void SetsExceptionMessage()
            {
                // Arrange
                var httpError = new HttpError { ExceptionMessage = "myExceptionMessage" };
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Act
                var exceptionMessage = ex.ExceptionMessage;

                // Assert
                Assert.Equal("myExceptionMessage", exceptionMessage);
            }

            [Fact]
            public void SetsExceptionType()
            {
                // Arrange
                var httpError = new HttpError { ExceptionType = "myExceptionType" };
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Act
                var exceptionType = ex.ExceptionType;

                // Assert
                Assert.Equal("myExceptionType", exceptionType);
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var httpError = new HttpError { ExceptionType = "myExceptionType" };
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Assert
                Assert.IsAssignableFrom<HttpErrorException>(ex);
            }
        }
    }
}
