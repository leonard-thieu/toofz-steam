using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpRequestStatusExceptionTests
    {
        [TestClass]
        public class Constructor_StatusCode
        {
            [TestMethod]
            public void SetsStatusCode()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;

                // Act
                var ex = new HttpRequestStatusException(statusCode);

                // Assert
                Assert.AreEqual(statusCode, ex.StatusCode);
            }
        }

        [TestClass]
        public class Constructor_Message_StatusCode
        {
            [TestMethod]
            public void SetsMessage()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;

                // Act
                var ex = new HttpRequestStatusException(message, statusCode);

                // Assert
                Assert.AreEqual(message, ex.Message);
            }

            [TestMethod]
            public void SetsStatusCode()
            {
                // Arrange
                var message = "myMessage";
                var statusCode = HttpStatusCode.BadGateway;

                // Act
                var ex = new HttpRequestStatusException(message, statusCode);

                // Assert
                Assert.AreEqual(statusCode, ex.StatusCode);
            }
        }

        [TestClass]
        public class RequestUriProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var ex = new HttpRequestStatusException(statusCode);
                var uri = new Uri("http://example.org/");

                // Act
                ex.RequestUri = uri;

                // Assert
                Assert.AreEqual(uri, ex.RequestUri);
            }
        }

        [TestClass]
        public class ResponseContent
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var statusCode = HttpStatusCode.BadGateway;
                var ex = new HttpRequestStatusException(statusCode);

                // Act
                ex.ResponseContent = "myContent";

                // Assert
                Assert.AreEqual("myContent", ex.ResponseContent);
            }
        }
    }
}
