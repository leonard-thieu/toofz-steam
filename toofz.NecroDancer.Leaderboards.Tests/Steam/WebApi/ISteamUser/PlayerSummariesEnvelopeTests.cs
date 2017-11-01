using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    public class PlayerSummariesEnvelopeTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutResponse_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummariesEnvelopeWithoutResponse;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummariesEnvelope>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummariesEnvelope;

                // Act
                var playerSummariesEnvelope = JsonConvert.DeserializeObject<PlayerSummariesEnvelope>(json);

                // Assert
                Assert.IsAssignableFrom<PlayerSummariesEnvelope>(playerSummariesEnvelope);
                Assert.IsAssignableFrom<PlayerSummaries>(playerSummariesEnvelope.Response);
            }
        }
    }
}
