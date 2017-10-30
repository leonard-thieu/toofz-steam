using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class EntryTests
    {
        [TestClass]
        public class LeaderboardIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { LeaderboardId = 123 };

                // Act -> Assert
                Assert.AreEqual(123, entry.LeaderboardId);
            }
        }

        [TestClass]
        public class LeaderboardProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard();
                var entry = new Entry { Leaderboard = leaderboard };

                // Act -> Assert
                Assert.AreEqual(leaderboard, entry.Leaderboard);
            }
        }

        [TestClass]
        public class RankProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Rank = 321 };

                // Act -> Assert
                Assert.AreEqual(321, entry.Rank);
            }
        }

        [TestClass]
        public class SteamIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { SteamId = 459687893 };

                // Act -> Assert
                Assert.AreEqual(459687893L, entry.SteamId);
            }
        }

        [TestClass]
        public class PlayerProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player();
                var entry = new Entry { Player = player };

                // Act -> Assert
                Assert.AreEqual(player, entry.Player);
            }
        }

        [TestClass]
        public class ReplayIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { ReplayId = 239847589234 };

                // Act -> Assert
                Assert.AreEqual(239847589234L, entry.ReplayId);
            }
        }

        [TestClass]
        public class ScoreProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Score = 10 };

                // Act -> Assert
                Assert.AreEqual(10, entry.Score);
            }
        }

        [TestClass]
        public class ZoneProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Zone = 1 };

                // Act -> Assert
                Assert.AreEqual(1, entry.Zone);
            }
        }

        [TestClass]
        public class LevelProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Level = 2 };

                // Act -> Assert
                Assert.AreEqual(2, entry.Level);
            }
        }
    }
}
