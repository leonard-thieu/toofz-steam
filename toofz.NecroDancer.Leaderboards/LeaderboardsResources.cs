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

                return response.Categories;
            }
        }

        sealed class CategoriesResponse
        {
            [JsonProperty("categories")]
            public Categories Categories { get; set; }
        }

        #endregion

        #region Leaderboard Headers

        public static LeaderboardHeaders ReadLeaderboardHeaders()
        {
            var serializer = new JsonSerializer();
            using (var sr = new StringReader(Resources.LeaderboardHeaders))
            {
                var response = ((LeaderboardHeadersResponse)serializer.Deserialize(sr, typeof(LeaderboardHeadersResponse)));

                return response.Leaderboards;
            }
        }

        sealed class LeaderboardHeadersResponse
        {
            [JsonProperty("leaderboards")]
            public LeaderboardHeaders Leaderboards { get; } = new LeaderboardHeaders();
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

                return response.Leaderboards;
            }
        }

        sealed class DailyLeaderboardHeadersResponse
        {
            [JsonProperty("leaderboards")]
            public DailyLeaderboardHeaders Leaderboards { get; } = new DailyLeaderboardHeaders();
        }

        #endregion
    }
}
