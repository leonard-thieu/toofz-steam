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
        public class Deserialization
        {
            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummaries;

                // Act
                var playerSummaries = JsonConvert.DeserializeObject<PlayerSummariesEnvelope>(json);
                var response = playerSummaries.Response;

                // Assert
                Assert.IsInstanceOfType(response, typeof(PlayerSummaries));
                Assert.AreEqual(1, response.Players.Count());
            }
        }
    }
}
