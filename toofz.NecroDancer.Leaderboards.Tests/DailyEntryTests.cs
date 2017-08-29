using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class DailyEntryTests
    {
        [TestClass]
        public class LeaderboardIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { LeaderboardId = 234 };

                // Act -> Assert
                Assert.AreEqual(234, dailyEntry.LeaderboardId);
            }
        }

        [TestClass]
        public class LeaderboardProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new DailyLeaderboard();
                var dailyEntry = new DailyEntry { Leaderboard = leaderboard };

                // Act -> Assert
                Assert.AreEqual(leaderboard, dailyEntry.Leaderboard);
            }
        }

        [TestClass]
        public class RankProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Rank = 345 };

                // Act -> Assert
                Assert.AreEqual(345, dailyEntry.Rank);
            }
        }

        [TestClass]
        public class SteamIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { SteamId = 765 };

                // Act -> Assert
                Assert.AreEqual(765L, dailyEntry.SteamId);

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
                var dailyEntry = new DailyEntry { Player = player };

                // Act -> Assert
                Assert.AreEqual(player, dailyEntry.Player);
            }
        }

        [TestClass]
        public class ReplayIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { ReplayId = 9082374 };

                // Act -> Assert
                Assert.AreEqual(9082374L, dailyEntry.ReplayId);
            }
        }

        [TestClass]
        public class ScoreProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Score = 10 };

                // Act -> Assert
                Assert.AreEqual(10, dailyEntry.Score);
            }
        }

        [TestClass]
        public class ZoneProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Zone = 1 };

                // Act -> Assert
                Assert.AreEqual(1, dailyEntry.Zone);
            }
        }

        [TestClass]
        public class LevelProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Level = 2 };

                // Act -> Assert
                Assert.AreEqual(2, dailyEntry.Level);
            }
        }
    }
}
