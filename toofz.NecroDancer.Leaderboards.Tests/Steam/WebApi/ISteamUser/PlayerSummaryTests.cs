using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    class PlayerSummaryTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutSteamId_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutSteamId;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutCommunityVisibilityState_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutCommunityVisibilityState;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutPersonaName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutPersonaName;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutLastLogOff_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutLastLogOff;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutProfileUrl_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutProfileUrl;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutAvatar_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutAvatar;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutAvatarMedium_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutAvatarMedium;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutAvatarFull_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutAvatarFull;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void WithoutPersonaState_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutPersonaState;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummary;

                // Act
                var playerSummary = JsonConvert.DeserializeObject<PlayerSummary>(json);

                // Assert
                Assert.AreEqual(76561197960435530, playerSummary.SteamId);
                Assert.AreEqual(3, playerSummary.CommunityVisibilityState);
                Assert.AreEqual(1, playerSummary.ProfileState);
                Assert.AreEqual("Robin", playerSummary.PersonaName);
                Assert.AreEqual(1501731437, playerSummary.LastLogOff);
                Assert.AreEqual("http://steamcommunity.com/id/robinwalker/", playerSummary.ProfileUrl);
                Assert.AreEqual("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f1/f1dd60a188883caf82d0cbfccfe6aba0af1732d4.jpg", playerSummary.Avatar);
                Assert.AreEqual("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f1/f1dd60a188883caf82d0cbfccfe6aba0af1732d4_medium.jpg", playerSummary.AvatarMedium);
                Assert.AreEqual("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f1/f1dd60a188883caf82d0cbfccfe6aba0af1732d4_full.jpg", playerSummary.AvatarFull);
                Assert.AreEqual(0, playerSummary.PersonaState);
                Assert.AreEqual("Robin Walker", playerSummary.RealName);
                Assert.AreEqual(103582791429521412U, playerSummary.PrimaryClanId);
                Assert.AreEqual(1063407589, playerSummary.TimeCreated);
                Assert.AreEqual(0, playerSummary.PersonaStateFlags);
                Assert.AreEqual("US", playerSummary.LocCountryCode);
                Assert.AreEqual("WA", playerSummary.LocStateCode);
                Assert.AreEqual(3961, playerSummary.LocCityId);
            }
        }
    }
}
