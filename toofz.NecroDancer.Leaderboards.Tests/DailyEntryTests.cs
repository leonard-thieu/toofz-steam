using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class DailyEntryTests
    {
        public class LeaderboardIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { LeaderboardId = 234 };

                // Act -> Assert
                Assert.Equal(234, dailyEntry.LeaderboardId);
            }
        }

        public class LeaderboardProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new DailyLeaderboard();
                var dailyEntry = new DailyEntry { Leaderboard = leaderboard };

                // Act -> Assert
                Assert.Equal(leaderboard, dailyEntry.Leaderboard);
            }
        }

        public class RankProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Rank = 345 };

                // Act -> Assert
                Assert.Equal(345, dailyEntry.Rank);
            }
        }

        public class SteamIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { SteamId = 765 };

                // Act -> Assert
                Assert.Equal(765L, dailyEntry.SteamId);

            }
        }

        public class PlayerProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player();
                var dailyEntry = new DailyEntry { Player = player };

                // Act -> Assert
                Assert.Equal(player, dailyEntry.Player);
            }
        }

        public class ReplayIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { ReplayId = 9082374 };

                // Act -> Assert
                Assert.Equal(9082374L, dailyEntry.ReplayId);
            }
        }

        public class ReplayProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var replay = new Replay();
                var dailyEntry = new DailyEntry();

                // Act
                dailyEntry.Replay = replay;
                var replay2 = dailyEntry.Replay;

                // Assert
                Assert.Same(replay, replay2);
            }
        }

        public class ScoreProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Score = 10 };

                // Act -> Assert
                Assert.Equal(10, dailyEntry.Score);
            }
        }

        public class ZoneProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Zone = 1 };

                // Act -> Assert
                Assert.Equal(1, dailyEntry.Zone);
            }
        }

        public class LevelProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyEntry = new DailyEntry { Level = 2 };

                // Act -> Assert
                Assert.Equal(2, dailyEntry.Level);
            }
        }
    }
}
