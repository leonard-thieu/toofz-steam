using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardsResourcesTests
    {
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
