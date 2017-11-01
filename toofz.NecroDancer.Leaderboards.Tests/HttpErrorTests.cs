using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class HttpErrorTests
    {
        public class Serialization
        {
            [Fact]
            public void HttpErrorWithoutMessage_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.HttpErrorWithoutMessage;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<HttpError>(json);
                });
            }

            [Fact]
            public void HttpErrorWithMessage_Deserializes()
            {
                // Arrange
                var json = Resources.HttpErrorWithMessage;

                // Act
                var httpError = JsonConvert.DeserializeObject<HttpError>(json);

                // Assert
                Assert.IsAssignableFrom<HttpError>(httpError);
                Assert.Equal("An error has occurred.", httpError.Message);
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.HttpError;

                // Act
                var httpError = JsonConvert.DeserializeObject<HttpError>(json);

                // Assert
                Assert.IsAssignableFrom<HttpError>(httpError);
                Assert.Equal("An error has occurred.", httpError.Message);
                Assert.NotNull(httpError.ExceptionMessage);
                Assert.NotNull(httpError.ExceptionType);
                Assert.NotNull(httpError.StackTrace);
                Assert.NotNull(httpError.InnerException);
            }
        }
    }
}
