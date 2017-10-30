using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class DailyLeaderboardTests
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
        public class NameProperty
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var name = "myName";
                var dailyLeaderboard = new DailyLeaderboard();

                // Act
                dailyLeaderboard.Name = name;
                var name2 = dailyLeaderboard.Name;

                // Assert
                Assert.AreEqual(name, name2);
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
                var dailyLeaderboard = new DailyLeaderboard();

                // Act
                dailyLeaderboard.DisplayName = displayName;
                var displayName2 = dailyLeaderboard.DisplayName;

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
                var dailyLeaderboard = new DailyLeaderboard { IsProduction = true };

                // Act -> Assert
                Assert.IsTrue(dailyLeaderboard.IsProduction);
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
                var dailyLeaderboard = new DailyLeaderboard { Product = product };
                var product2 = dailyLeaderboard.Product;

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
                var dailyLeaderboard = new DailyLeaderboard { ProductId = 1 };

                // Act -> Assert
                Assert.AreEqual(1, dailyLeaderboard.ProductId);
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
    }
}
