using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ReplayTests
    {
        public class ReplayIdProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { ReplayId = 32874823748 };

                // Act -> Assert
                Assert.Equal(32874823748L, replay.ReplayId);
            }
        }


        public class ErrorCodeProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { ErrorCode = 23847 };

                // Act -> Assert
                Assert.Equal(23847, replay.ErrorCode);
            }
        }


        public class SeedProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { Seed = 234235 };

                // Act -> Assert
                Assert.Equal(234235, replay.Seed);
            }
        }


        public class VersionProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { Version = 12 };

                // Act -> Assert
                Assert.Equal(12, replay.Version);
            }
        }


        public class KilledByProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { KilledBy = "A scary enemy" };

                // Act -> Assert
                Assert.Equal("A scary enemy", replay.KilledBy);
            }
        }


        public class UriProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { Uri = "http://example.org/" };

                // Act -> Assert
                Assert.Equal("http://example.org/", replay.Uri);
            }
        }
    }
}
