using System;
using System.Net;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class HttpRequestStatusExceptionTests
    {
        public class Constructor_HttpStatusCode_Uri
        {
            [Fact]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                Uri requestUri = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var ex = new HttpRequestStatusException(statusCode, requestUri);
                });
            }

            [Fact]
            public void SetsStatusCode()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Assert
                Assert.Equal(statusCode, ex.StatusCode);
            }

            [Fact]
            public void SetsRequestUri()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Assert
                Assert.Equal(requestUri, ex.RequestUri);
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Assert
                Assert.IsAssignableFrom<HttpRequestStatusException>(ex);
            }
        }
        
        public class Constructor_String_HttpStatusCode_Uri
        {
            [Fact]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                Uri requestUri = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var ex = new HttpRequestStatusException(message, statusCode, requestUri);
                });
            }

            [Fact]
            public void SetsMessage()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.Equal(message, ex.Message);
            }

            [Fact]
            public void SetsStatusCode()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.Equal(statusCode, ex.StatusCode);
            }

            [Fact]
            public void SetsRequestUri()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.Equal(requestUri, ex.RequestUri);
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.IsAssignableFrom<HttpRequestStatusException>(ex);
            }
        }
        
        public class Constructor_String_HttpStatusCode_Uri_String
        {
            [Fact]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                Uri requestUri = null;
                var responseContent = "myResponseContent";

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new HttpRequestStatusException(message, statusCode, requestUri, responseContent);
                });
            }

            [Fact]
            public void SetsMessage()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");
                var responseContent = "myResponseContent";

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri, responseContent);

                // Assert
                Assert.Equal(message, ex.Message);
            }

            [Fact]
            public void SetsStatusCode()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");
                var responseContent = "myResponseContent";

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri, responseContent);

                // Assert
                Assert.Equal(statusCode, ex.StatusCode);
            }

            [Fact]
            public void SetsRequestUri()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");
                var responseContent = "myResponseContent";

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri, responseContent);

                // Assert
                Assert.Equal(requestUri, ex.RequestUri);
            }

            [Fact]
            public void SetsResponseContent()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");
                var responseContent = "myResponseContent";

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri, responseContent);

                // Assert
                Assert.Equal(responseContent, ex.ResponseContent);
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");
                var responseContent = "myResponseContent";

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri, responseContent);

                // Assert
                Assert.IsAssignableFrom<HttpRequestStatusException>(ex);
            }
        }
    }
}
