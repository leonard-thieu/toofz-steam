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
    }
}
