using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class DailyLeaderboardTests
    {
        [TestClass]
        public class LeaderboardIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyLeaderboard = new DailyLeaderboard { LeaderboardId = 43895 };

                // Act -> Assert
                Assert.AreEqual(43895, dailyLeaderboard.LeaderboardId);
            }
        }

        [TestClass]
        public class EntriesProperty
        {
            [TestMethod]
            public void GetBehavior()
            {
                // Arrange
                var dailyLeaderboard = new DailyLeaderboard();

                // Act -> Assert
                Assert.IsInstanceOfType(dailyLeaderboard.Entries, typeof(List<DailyEntry>));
            }
        }

        [TestClass]
        public class LastUpdateProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyLeaderboard = new DailyLeaderboard { LastUpdate = new DateTime(2017, 8, 28, 16, 27, 58) };

                // Act -> Assert
                Assert.AreEqual(new DateTime(2017, 8, 28, 16, 27, 58), dailyLeaderboard.LastUpdate);
            }
        }

        [TestClass]
        public class DateProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyLeaderboard = new DailyLeaderboard { Date = new DateTime(2017, 8, 28) };

                // Act -> Assert
                Assert.AreEqual(new DateTime(2017, 8, 28), dailyLeaderboard.Date);
            }
        }

        [TestClass]
        public class ProductIdProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyLeaderboard = new DailyLeaderboard { ProductId = 1 };

                // Act -> Assert
                Assert.AreEqual(1, dailyLeaderboard.ProductId);
            }
        }

        [TestClass]
        public class IsProductionProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange -> Act
                var dailyLeaderboard = new DailyLeaderboard { IsProduction = true };

                // Act -> Assert
                Assert.IsTrue(dailyLeaderboard.IsProduction);
            }
        }
    }
}
