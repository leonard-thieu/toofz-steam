using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class ReplayTests
    {
        [TestClass]
        public class ReplayIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { ReplayId = 32874823748 };

                // Act -> Assert
                Assert.AreEqual(32874823748L, replay.ReplayId);
            }
        }

        [TestClass]
        public class ErrorCodeProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { ErrorCode = 23847 };

                // Act -> Assert
                Assert.AreEqual(23847, replay.ErrorCode);
            }
        }

        [TestClass]
        public class SeedProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { Seed = 234235 };

                // Act -> Assert
                Assert.AreEqual(234235, replay.Seed);
            }
        }

        [TestClass]
        public class VersionProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { Version = 12 };

                // Act -> Assert
                Assert.AreEqual(12, replay.Version);
            }
        }

        [TestClass]
        public class KilledByProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { KilledBy = "A scary enemy" };

                // Act -> Assert
                Assert.AreEqual("A scary enemy", replay.KilledBy);
            }
        }

        [TestClass]
        public class UriProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var replay = new Replay { Uri = "http://example.org/" };

                // Act -> Assert
                Assert.AreEqual("http://example.org/", replay.Uri);
            }
        }
    }
}
