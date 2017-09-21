using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpErrorTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void HttpErrorWithoutMessage_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.HttpErrorWithoutMessage;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<HttpError>(json);
                });
            }

            [TestMethod]
            public void HttpErrorWithMessage_Deserializes()
            {
                // Arrange
                var json = Resources.HttpErrorWithMessage;

                // Act
                var httpError = JsonConvert.DeserializeObject<HttpError>(json);

                // Assert
                Assert.IsInstanceOfType(httpError, typeof(HttpError));
                Assert.AreEqual("An error has occurred.", httpError.Message);
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.HttpError;

                // Act
                var httpError = JsonConvert.DeserializeObject<HttpError>(json);

                // Assert
                Assert.IsInstanceOfType(httpError, typeof(HttpError));
                Assert.AreEqual("An error has occurred.", httpError.Message);
                Assert.IsNotNull(httpError.ExceptionMessage);
                Assert.IsNotNull(httpError.ExceptionType);
                Assert.IsNotNull(httpError.StackTrace);
                Assert.IsNotNull(httpError.InnerException);
            }
        }
    }
}
