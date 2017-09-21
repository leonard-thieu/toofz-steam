using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    class PlayerSummariesEnvelopeTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutResponse_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummariesEnvelopeWithoutResponse;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummariesEnvelope>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummariesEnvelope;

                // Act
                var playerSummariesEnvelope = JsonConvert.DeserializeObject<PlayerSummariesEnvelope>(json);

                // Assert
                Assert.IsInstanceOfType(playerSummariesEnvelope, typeof(PlayerSummariesEnvelope));
                Assert.IsInstanceOfType(playerSummariesEnvelope.Response, typeof(PlayerSummaries));
            }
        }
    }
}
