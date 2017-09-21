using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class OAuth2BearerTokenTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutAccessToken_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutAccessToken;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [TestMethod]
            public void WithoutTokenType_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutTokenType;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [TestMethod]
            public void WithoutExpiresIn_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutExpiresIn;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [TestMethod]
            public void WithoutUserName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutUserName;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [TestMethod]
            public void WithoutIssued_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutIssued;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [TestMethod]
            public void WithoutExpires_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.OAuth2BearerTokenWithoutExpires;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<OAuth2BearerToken>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.OAuth2BearerToken;

                // Act
                var bearerToken = JsonConvert.DeserializeObject<OAuth2BearerToken>(json);

                // Assert
                Assert.IsInstanceOfType(bearerToken, typeof(OAuth2BearerToken));
            }
        }
    }
}
