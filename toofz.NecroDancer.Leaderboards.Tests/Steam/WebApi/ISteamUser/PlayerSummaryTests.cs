using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamUser
{
    public class PlayerSummaryTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutSteamId_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutSteamId;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutCommunityVisibilityState_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutCommunityVisibilityState;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutPersonaName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutPersonaName;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutLastLogOff_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutLastLogOff;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutProfileUrl_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutProfileUrl;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutAvatar_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutAvatar;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutAvatarMedium_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutAvatarMedium;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutAvatarFull_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutAvatarFull;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void WithoutPersonaState_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayerSummaryWithoutPersonaState;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerSummary>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayerSummary;

                // Act
                var playerSummary = JsonConvert.DeserializeObject<PlayerSummary>(json);

                // Assert
                Assert.Equal(76561197960435530, playerSummary.SteamId);
                Assert.Equal(3, playerSummary.CommunityVisibilityState);
                Assert.Equal(1, playerSummary.ProfileState);
                Assert.Equal("Robin", playerSummary.PersonaName);
                Assert.Equal(1501731437, playerSummary.LastLogOff);
                Assert.Equal("http://steamcommunity.com/id/robinwalker/", playerSummary.ProfileUrl);
                Assert.Equal("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f1/f1dd60a188883caf82d0cbfccfe6aba0af1732d4.jpg", playerSummary.Avatar);
                Assert.Equal("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f1/f1dd60a188883caf82d0cbfccfe6aba0af1732d4_medium.jpg", playerSummary.AvatarMedium);
                Assert.Equal("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f1/f1dd60a188883caf82d0cbfccfe6aba0af1732d4_full.jpg", playerSummary.AvatarFull);
                Assert.Equal(0, playerSummary.PersonaState);
                Assert.Equal("Robin Walker", playerSummary.RealName);
                Assert.Equal(103582791429521412U, playerSummary.PrimaryClanId);
                Assert.Equal(1063407589, playerSummary.TimeCreated);
                Assert.Equal(0, playerSummary.PersonaStateFlags);
                Assert.Equal("US", playerSummary.LocCountryCode);
                Assert.Equal("WA", playerSummary.LocStateCode);
                Assert.Equal(3961, playerSummary.LocCityId);
            }
        }
    }
}
