using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    class PlayerSummariesEnvelopeTests
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

                // Assert
                Assert.IsInstanceOfType(playerSummaries, typeof(PlayerSummariesEnvelope));
                Assert.IsNotNull(playerSummaries.Response);
            }
        }
    }
}
