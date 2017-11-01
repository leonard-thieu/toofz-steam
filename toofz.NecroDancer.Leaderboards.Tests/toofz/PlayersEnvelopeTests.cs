using System.Linq;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class PlayersEnvelopeTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutTotal_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersEnvelopeWithoutTotal;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayersEnvelope>(json);
                });
            }

            [Fact]
            public void WithoutPlayers_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersEnvelopeWithoutPlayers;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayersEnvelope>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayersEnvelope;

                // Act
                var playersEnvelope = JsonConvert.DeserializeObject<PlayersEnvelope>(json);

                // Assert
                Assert.Equal(453681, playersEnvelope.Total);
                var players = playersEnvelope.Players.ToList();
                Assert.Equal(20, players.Count);
                foreach (var p in players)
                {
                    Assert.IsAssignableFrom<PlayerDTO>(p);
                }
            }
        }
    }
}
