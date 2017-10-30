using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class PlayerTests
    {
        [TestClass]
        public class SteamIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { SteamId = 34897238 };

                // Act -> Assert
                Assert.AreEqual(34897238L, player.SteamId);
            }
        }

        [TestClass]
        public class ExistsProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { Exists = true };

                // Act - Assert
                Assert.AreEqual(true, player.Exists);
            }
        }

        [TestClass]
        public class NameProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { Name = "MYnAME" };

                // Act -> Assert
                Assert.AreEqual("MYnAME", player.Name);
            }
        }

        [TestClass]
        public class AvatarProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { Avatar = "http://my.avatar.url/" };

                // Act -> Assert
                Assert.AreEqual("http://my.avatar.url/", player.Avatar);
            }
        }

        [TestClass]
        public class LastUpdateProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { LastUpdate = new DateTime(2017, 8, 30, 18, 53, 49) };

                // Act -> Assert
                Assert.AreEqual(new DateTime(2017, 8, 30, 18, 53, 49), player.LastUpdate);
            }
        }

        [TestClass]
        public class EntriesProperty
        {
            [TestMethod]
            public void GetBehavior()
            {
                // Arrange
                var player = new Player();

                // Act -> Assert
                Assert.IsInstanceOfType(player.Entries, typeof(List<Entry>));
            }
        }
    }
}
