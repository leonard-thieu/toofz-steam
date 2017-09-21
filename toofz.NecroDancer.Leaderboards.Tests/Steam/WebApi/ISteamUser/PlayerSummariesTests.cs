using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    class PlayerSummariesTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutPlayers_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummariesWithoutPlayers;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummaries>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummaries;

                // Act
                var playerSummaries = JsonConvert.DeserializeObject<PlayerSummaries>(json);

                // Assert
                Assert.IsInstanceOfType(playerSummaries, typeof(PlayerSummaries));
                var players = playerSummaries.Players.ToList();
                Assert.AreEqual(1, players.Count);
                CollectionAssert.AllItemsAreInstancesOfType(players, typeof(PlayerSummary));
            }
        }
    }
}
