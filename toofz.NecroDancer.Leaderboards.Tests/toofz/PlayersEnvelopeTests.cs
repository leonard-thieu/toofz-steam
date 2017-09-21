using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class PlayersEnvelopeTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutTotal_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersEnvelopeWithoutTotal;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayersEnvelope>(json);
                });
            }

            [TestMethod]
            public void WithoutPlayers_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersEnvelopeWithoutPlayers;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayersEnvelope>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayersEnvelope;

                // Act
                var playersEnvelope = JsonConvert.DeserializeObject<PlayersEnvelope>(json);

                // Assert
                Assert.AreEqual(453681, playersEnvelope.Total);
                var players = playersEnvelope.Players.ToList();
                Assert.AreEqual(20, players.Count);
                CollectionAssert.AllItemsAreInstancesOfType(players, typeof(PlayerDTO));
            }
        }
    }
}
