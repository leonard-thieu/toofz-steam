using System;
using System.Net;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class HttpErrorExceptionTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void HttpErrorIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpError httpError = null;
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");

                // Act -> Assert
                var ex = Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new HttpErrorException(httpError, statusCode, requestUri);
                });
                Assert.AreEqual("httpError", ex.ParamName);
            }

            [TestMethod]
            public void RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var httpError = new HttpError();
                var statusCode = HttpStatusCode.NotFound;
                Uri requestUri = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new HttpErrorException(httpError, statusCode, requestUri);
                });
            }

            [TestMethod]
            public void SetsMessage()
            {
                // Arrange
                var httpError = new HttpError("myMessage");
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Act
                var message = ex.Message;

                // Assert
                Assert.AreEqual("myMessage", message);
            }

            [TestMethod]
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
                Assert.AreEqual("myStackTrace", stackTrace);
            }

            [TestMethod]
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
                Assert.AreEqual("myExceptionMessage", exceptionMessage);
            }

            [TestMethod]
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
                Assert.AreEqual("myExceptionType", exceptionType);
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var httpError = new HttpError { ExceptionType = "myExceptionType" };
                var statusCode = HttpStatusCode.NotFound;
                var requestUri = new Uri("http://localhost/");

                // Act
                var ex = new HttpErrorException(httpError, statusCode, requestUri);

                // Assert
                Assert.IsInstanceOfType(ex, typeof(HttpErrorException));
            }
        }
    }
}
