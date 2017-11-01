using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class OAuth2BearerTokenTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutAccessToken_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutAccessToken;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [Fact]
            public void WithoutTokenType_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutTokenType;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [Fact]
            public void WithoutExpiresIn_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutExpiresIn;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [Fact]
            public void WithoutUserName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutUserName;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [Fact]
            public void WithoutIssued_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutIssued;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [Fact]
            public void WithoutExpires_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutExpires;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.OAuth2BearerToken;

                // Act
                var bearerToken = JsonConvert.DeserializeObject<OAuth2BearerToken>(json);

                // Assert
                Assert.IsAssignableFrom<OAuth2BearerToken>(bearerToken);
            }
        }
    }
}
