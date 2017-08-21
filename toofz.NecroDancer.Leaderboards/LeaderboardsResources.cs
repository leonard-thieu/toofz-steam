using System.IO;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Properties;

namespace toofz.NecroDancer.Leaderboards
{
    public static class LeaderboardsResources
    {
        #region Leaderboard Categories

        public static Categories ReadLeaderboardCategories()
        {
            var serializer = new JsonSerializer();
            using (var sr = new StringReader(Resources.LeaderboardCategories))
            {
                var response = ((CategoriesResponse)serializer.Deserialize(sr, typeof(CategoriesResponse)));

                return response.categories;
            }
        }

        sealed class CategoriesResponse
        {
            public Categories categories { get; set; }
        }

        #endregion

        #region Leaderboard Headers

        public static LeaderboardHeaders ReadLeaderboardHeaders()
        {
            var serializer = new JsonSerializer();
            using (var sr = new StringReader(Resources.LeaderboardHeaders))
            {
                var response = ((LeaderboardHeadersResponse)serializer.Deserialize(sr, typeof(LeaderboardHeadersResponse)));

                return response.leaderboards;
            }
        }

        sealed class LeaderboardHeadersResponse
        {
            public LeaderboardHeaders leaderboards { get; } = new LeaderboardHeaders();
        }

        #endregion

        #region Daily Leaderboard Headers

        public static DailyLeaderboardHeaders ReadDailyLeaderboardHeaders()
        {
            var serializer = new JsonSerializer();
            using (var sr = new StringReader(Resources.DailyLeaderboardHeaders))
            {
                var response = ((DailyLeaderboardHeadersResponse)serializer.Deserialize(
                    sr,
                    typeof(DailyLeaderboardHeadersResponse)));

                return response.leaderboards;
            }
        }

        sealed class DailyLeaderboardHeadersResponse
        {
            public DailyLeaderboardHeaders leaderboards { get; } = new DailyLeaderboardHeaders();
        }

        #endregion
    }
}
