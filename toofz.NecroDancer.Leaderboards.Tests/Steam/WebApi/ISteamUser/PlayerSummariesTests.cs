using System.Linq;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    public class PlayerSummariesTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutPlayers_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummariesWithoutPlayers;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummaries>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummaries;

                // Act
                var playerSummaries = JsonConvert.DeserializeObject<PlayerSummaries>(json);

                // Assert
                Assert.IsAssignableFrom<PlayerSummaries>(playerSummaries);
                var players = playerSummaries.Players.ToList();
#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
                Assert.Equal(1, players.Count);
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.
                Assert.Collection(players, p => Assert.IsAssignableFrom<PlayerSummary>(p));
            }
        }
    }
}
