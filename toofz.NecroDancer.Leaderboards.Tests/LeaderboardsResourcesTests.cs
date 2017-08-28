using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardsResourcesTests
    {
        [TestClass]
        public class ReadLeaderboardCategories
        {
            [TestMethod]
            public void ReturnsCategories()
            {
                // Arrange -> Act
                var categories = LeaderboardsResources.ReadLeaderboardCategories();

                // Act -> Assert
                Assert.IsInstanceOfType(categories, typeof(Categories));
            }
        }

        [TestClass]
        public class ReadLeaderboardHeaders
        {
            [TestMethod]
            public void ReturnsLeaderboardHeaders()
            {
                // Arrange -> Act
                var leaderboardHeaders = LeaderboardsResources.ReadLeaderboardHeaders();

                // Act -> Assert
                Assert.IsInstanceOfType(leaderboardHeaders, typeof(LeaderboardHeaders));
            }
        }

        [TestClass]
        public class ReadDailyLeaderboardHeaders
        {
            [TestMethod]
            public void ReturnsDailyLeaderboardHeaders()
            {
                // Arrange -> Act
                var dailyLeaderboardHeaders = LeaderboardsResources.ReadDailyLeaderboardHeaders();

                // Act -> Assert
                Assert.IsInstanceOfType(dailyLeaderboardHeaders, typeof(DailyLeaderboardHeaders));
            }
        }
    }
}
