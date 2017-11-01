using System;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class PlayerDTOTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutId_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutId;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [Fact]
            public void WithoutDisplayName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutDisplayName;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [Fact]
            public void WithoutUpdatedAt_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutUpdatedAt;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [Fact]
            public void WithoutAvatar_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutAvatar;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayersDTO;

                // Act
                var player = JsonConvert.DeserializeObject<PlayerDTO>(json);

                // Assert
                Assert.IsAssignableFrom<PlayerDTO>(player);
                Assert.Equal(76561198020278823, player.Id);
                Assert.Equal("Mr.moneybottoms", player.DisplayName);
                Assert.Equal(DateTime.Parse("2017-09-13T12:48:01.35Z").ToUniversalTime(), player.UpdatedAt);
                Assert.Equal("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/cb/cb555a66da219db0dd0504b69ccbd810678fe203.jpg", player.Avatar);
            }
        }
    }
}
