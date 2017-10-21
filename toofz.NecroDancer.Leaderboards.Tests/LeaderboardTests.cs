using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardTests
    {
        [TestClass]
        public class LeaderboardIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { LeaderboardId = 43895 };

                // Act -> Assert
                Assert.AreEqual(43895, leaderboard.LeaderboardId);
            }
        }

        [TestClass]
        public class EntriesProperty
        {
            [TestMethod]
            public void GetBehavior()
            {
                // Arrange
                var leaderboard = new Leaderboard();

                // Act -> Assert
                Assert.IsInstanceOfType(leaderboard.Entries, typeof(List<Entry>));
            }
        }

        [TestClass]
        public class LastUpdateProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { LastUpdate = new DateTime(2017, 8, 28, 16, 27, 58) };

                // Act -> Assert
                Assert.AreEqual(new DateTime(2017, 8, 28, 16, 27, 58), leaderboard.LastUpdate);
            }
        }

        [TestClass]
        public class DisplayNameProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var displayName = "MyDisplayName";
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.DisplayName = displayName;
                var displayName2 = leaderboard.DisplayName;

                // Assert
                Assert.AreEqual(displayName, displayName2);
            }
        }

        [TestClass]
        public class IsProductionProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { IsProduction = true };

                // Act -> Assert
                Assert.IsTrue(leaderboard.IsProduction);
            }
        }

        [TestClass]
        public class ProductProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var product = new Product(1, "myProduct", "MyProduct");

                // Act
                var leaderboard = new Leaderboard { Product = product };
                var product2 = leaderboard.Product;

                // Assert
                Assert.AreSame(product, product2);
            }
        }

        [TestClass]
        public class ProductIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { ProductId = 1 };

                // Act -> Assert
                Assert.AreEqual(1, leaderboard.ProductId);
            }
        }

        [TestClass]
        public class ModeProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var mode = new Mode(1, "myMode", "MyMode");

                // Act
                var leaderboard = new Leaderboard { Mode = mode };
                var mode2 = leaderboard.Mode;

                // Assert
                Assert.AreSame(mode, mode2);
            }
        }

        [TestClass]
        public class ModeIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var modeId = 1;
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.ModeId = modeId;
                var modeId2 = leaderboard.ModeId;

                // Assert
                Assert.AreEqual(modeId, modeId2);
            }
        }

        [TestClass]
        public class RunProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var run = new Run(1, "myRun", "MyRun");
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.Run = run;
                var run2 = leaderboard.Run;

                // Assert
                Assert.AreSame(run, run2);
            }
        }

        [TestClass]
        public class RunIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { RunId = 2 };

                // Act -> Assert
                Assert.AreEqual(2, leaderboard.RunId);
            }
        }

        [TestClass]
        public class CharacterProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var character = new Character(1, "myCharacter", "MyCharacter");
                var leaderboard = new Leaderboard();

                // Act
                leaderboard.Character = character;
                var character2 = leaderboard.Character;

                // Assert
                Assert.AreSame(character, character2);
            }
        }

        [TestClass]
        public class CharacterIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { CharacterId = 9 };

                // Act -> Assert
                Assert.AreEqual(9, leaderboard.CharacterId);
            }
        }

        [TestClass]
        public class IsCustomMusicProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { IsCustomMusic = true };

                // Act -> Assert
                Assert.IsTrue(leaderboard.IsCustomMusic);
            }
        }

        [TestClass]
        public class IsCoOpProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard { IsCoOp = true };

                // Act -> Assert
                Assert.IsTrue(leaderboard.IsCoOp);
            }
        }
    }
}
