using Newtonsoft.Json;
using toofz.Steam.WebApi.ISteamUser;
using toofz.Steam.Tests.Properties;
using Xunit;

namespace toofz.Steam.Tests.WebApi.ISteamUser
{
    public class PlayerSummariesEnvelopeTests
    {
        public class Serialization
        {
            [DisplayFact(nameof(PlayerSummariesEnvelope.Response))]
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

            [DisplayFact]
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
