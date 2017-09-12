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
            public void SetsMessage()
            {
                // Arrange
                var httpError = new HttpError("myMessage");
                var ex = new HttpErrorException(httpError, HttpStatusCode.OK);

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
                var ex = new HttpErrorException(httpError, HttpStatusCode.OK);

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
                var ex = new HttpErrorException(httpError, HttpStatusCode.OK);

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
                var ex = new HttpErrorException(httpError, HttpStatusCode.OK);

                // Act
                var exceptionType = ex.ExceptionType;

                // Assert
                Assert.AreEqual("myExceptionType", exceptionType);
            }
        }
    }
}
