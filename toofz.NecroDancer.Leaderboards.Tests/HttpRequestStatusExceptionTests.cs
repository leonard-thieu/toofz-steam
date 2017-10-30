using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class HttpRequestStatusExceptionTests
    {
        [TestClass]
        public class Constructor_HttpStatusCode_Uri
        {
            [TestMethod]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                Uri requestUri = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    var ex = new HttpRequestStatusException(statusCode, requestUri);
                });
            }

            [TestMethod]
            public void SetsStatusCode()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Assert
                Assert.AreEqual(statusCode, ex.StatusCode);
            }

            [TestMethod]
            public void SetsRequestUri()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Assert
                Assert.AreEqual(requestUri, ex.RequestUri);
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(statusCode, requestUri);

                // Assert
                Assert.IsInstanceOfType(ex, typeof(HttpRequestStatusException));
            }
        }

        [TestClass]
        public class Constructor_String_HttpStatusCode_Uri
        {
            [TestMethod]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                Uri requestUri = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    var ex = new HttpRequestStatusException(message, statusCode, requestUri);
                });
            }

            [TestMethod]
            public void SetsMessage()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.AreEqual(message, ex.Message);
            }

            [TestMethod]
            public void SetsStatusCode()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.AreEqual(statusCode, ex.StatusCode);
            }

            [TestMethod]
            public void SetsRequestUri()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.AreEqual(requestUri, ex.RequestUri);
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpRequestStatusException(message, statusCode, requestUri);

                // Assert
                Assert.IsInstanceOfType(ex, typeof(HttpRequestStatusException));
            }
        }

        [TestClass]
        public class Constructor_String_HttpStatusCode_Uri_String
        {
            [TestMethod]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;
                Uri requestUri = null;
                var responseContent = "myResponseContent";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new HttpRequestStatusException(message, statusCode, requestUri, responseContent);
                });
            }

            [TestMethod]
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
                Assert.AreEqual(message, ex.Message);
            }

            [TestMethod]
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
                Assert.AreEqual(statusCode, ex.StatusCode);
            }

            [TestMethod]
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
                Assert.AreEqual(requestUri, ex.RequestUri);
            }

            [TestMethod]
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
                Assert.AreEqual(responseContent, ex.ResponseContent);
            }

            [TestMethod]
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
                Assert.IsInstanceOfType(ex, typeof(HttpRequestStatusException));
            }
        }
    }
}
