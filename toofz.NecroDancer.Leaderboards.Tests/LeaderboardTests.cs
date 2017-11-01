using System;
using System.Collections.Generic;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class LeaderboardTests
    {
        public class LeaderboardIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { LeaderboardId = 43895 };

                // Act -> Assert
                Assert.Equal(43895, leaderboard.LeaderboardId);
            }
        }

        public class EntriesProperty
        {
            [Fact]
            public void GetBehavior()
            {
                // Arrange
                var leaderboard = new Leaderboard();

                // Act -> Assert
                Assert.IsAssignableFrom<List<Entry>>(leaderboard.Entries);
            }
        }

        public class LastUpdateProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { LastUpdate = new DateTime(2017, 8, 28, 16, 27, 58) };

                // Act -> Assert
                Assert.Equal(new DateTime(2017, 8, 28, 16, 27, 58), leaderboard.LastUpdate);
            }
        }

        public class NameProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var name = "myName";
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.Name = name;
                var name2 = leaderboard.Name;

                // Assert
                Assert.Equal(name, name2);
            }
        }

        public class DisplayNameProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var displayName = "MyDisplayName";
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.DisplayName = displayName;
                var displayName2 = leaderboard.DisplayName;

                // Assert
                Assert.Equal(displayName, displayName2);
            }
        }

        public class IsProductionProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { IsProduction = true };

                // Act -> Assert
                Assert.True(leaderboard.IsProduction);
            }
        }

        public class ProductProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var product = new Product(1, "myProduct", "MyProduct");

                // Act
                var leaderboard = new Leaderboard { Product = product };
                var product2 = leaderboard.Product;

                // Assert
                Assert.Same(product, product2);
            }
        }

        public class ProductIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { ProductId = 1 };

                // Act -> Assert
                Assert.Equal(1, leaderboard.ProductId);
            }
        }

        public class ModeProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var mode = new Mode(1, "myMode", "MyMode");

                // Act
                var leaderboard = new Leaderboard { Mode = mode };
                var mode2 = leaderboard.Mode;

                // Assert
                Assert.Same(mode, mode2);
            }
        }

        public class ModeIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var modeId = 1;
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.ModeId = modeId;
                var modeId2 = leaderboard.ModeId;

                // Assert
                Assert.Equal(modeId, modeId2);
            }
        }

        public class RunProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var run = new Run(1, "myRun", "MyRun");
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.Run = run;
                var run2 = leaderboard.Run;

                // Assert
                Assert.Same(run, run2);
            }
        }

        public class RunIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { RunId = 2 };

                // Act -> Assert
                Assert.Equal(2, leaderboard.RunId);
            }
        }

        public class CharacterProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var character = new Character(1, "myCharacter", "MyCharacter");
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.Character = character;
                var character2 = leaderboard.Character;

                // Assert
                Assert.Same(character, character2);
            }
        }

        public class CharacterIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { CharacterId = 9 };

                // Act -> Assert
                Assert.Equal(9, leaderboard.CharacterId);
            }
        }

        public class IsCoOpProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { IsCoOp = true };

                // Act -> Assert
                Assert.True(leaderboard.IsCoOp);
            }
        }

        public class IsCustomMusicProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { IsCustomMusic = true };

                // Act -> Assert
                Assert.True(leaderboard.IsCustomMusic);
            }
        }
    }
}
