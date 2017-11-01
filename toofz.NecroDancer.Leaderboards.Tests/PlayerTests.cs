using System;
using System.Collections.Generic;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class PlayerTests
    {
        public class SteamIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { SteamId = 34897238 };

                // Act -> Assert
                Assert.Equal(34897238L, player.SteamId);
            }
        }


        public class ExistsProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { Exists = true };

                // Act - Assert
                Assert.True(player.Exists);
            }
        }


        public class NameProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { Name = "MYnAME" };

                // Act -> Assert
                Assert.Equal("MYnAME", player.Name);
            }
        }


        public class AvatarProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { Avatar = "http://my.avatar.url/" };

                // Act -> Assert
                Assert.Equal("http://my.avatar.url/", player.Avatar);
            }
        }


        public class LastUpdateProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player { LastUpdate = new DateTime(2017, 8, 30, 18, 53, 49) };

                // Act -> Assert
                Assert.Equal(new DateTime(2017, 8, 30, 18, 53, 49), player.LastUpdate);
            }
        }


        public class EntriesProperty
        {
            [Fact]
            public void GetBehavior()
            {
                // Arrange
                var player = new Player();

                // Act -> Assert
                Assert.IsAssignableFrom<List<Entry>>(player.Entries);
            }
        }
    }
}
