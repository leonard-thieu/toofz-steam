using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class EntryTests
    {
        public class LeaderboardIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { LeaderboardId = 123 };

                // Act -> Assert
                Assert.Equal(123, entry.LeaderboardId);
            }
        }

        public class LeaderboardProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var leaderboard = new Leaderboard();
                var entry = new Entry { Leaderboard = leaderboard };

                // Act -> Assert
                Assert.Equal(leaderboard, entry.Leaderboard);
            }
        }

        public class RankProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Rank = 321 };

                // Act -> Assert
                Assert.Equal(321, entry.Rank);
            }
        }

        public class SteamIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { SteamId = 459687893 };

                // Act -> Assert
                Assert.Equal(459687893L, entry.SteamId);
            }
        }

        public class PlayerProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var player = new Player();
                var entry = new Entry { Player = player };

                // Act -> Assert
                Assert.Equal(player, entry.Player);
            }
        }

        public class ReplayIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { ReplayId = 239847589234 };

                // Act -> Assert
                Assert.Equal(239847589234L, entry.ReplayId);
            }
        }

        public class ReplayProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var replay = new Replay();
                var entry = new Entry();

                // Act
                entry.Replay = replay;
                var replay2 = entry.Replay;

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
                var entry = new Entry { Score = 10 };

                // Act -> Assert
                Assert.Equal(10, entry.Score);
            }
        }

        public class ZoneProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Zone = 1 };

                // Act -> Assert
                Assert.Equal(1, entry.Zone);
            }
        }

        public class LevelProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var entry = new Entry { Level = 2 };

                // Act -> Assert
                Assert.Equal(2, entry.Level);
            }
        }
    }
}
