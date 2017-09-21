using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class PlayerDTOTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutId_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutId;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutDisplayName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutDisplayName;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutUpdatedAt_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutUpdatedAt;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutAvatar_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.PlayersDTOWithoutAvatar;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<PlayerDTO>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.PlayersDTO;

                // Act
                var player = JsonConvert.DeserializeObject<PlayerDTO>(json);

                // Assert
                Assert.IsInstanceOfType(player, typeof(PlayerDTO));
                Assert.AreEqual(76561198020278823, player.Id);
                Assert.AreEqual("Mr.moneybottoms", player.DisplayName);
                Assert.AreEqual(DateTime.Parse("2017-09-13T12:48:01.35Z").ToUniversalTime(), player.UpdatedAt);
                Assert.AreEqual("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/cb/cb555a66da219db0dd0504b69ccbd810678fe203.jpg", player.Avatar);
            }
        }
    }
}
