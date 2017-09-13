using System.Runtime.Serialization;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Properties;

namespace toofz.NecroDancer.Leaderboards
{
    public static class LeaderboardsResources
    {
        #region Leaderboard Categories

        public static Categories ReadLeaderboardCategories()
        {
            var response = JsonConvert.DeserializeObject<CategoriesResponse>(Resources.LeaderboardCategories);

            return response.Categories;
        }

        [DataContract]
        sealed class CategoriesResponse
        {
            [DataMember(Name = "categories")]
            public Categories Categories { get; set; }
        }

        #endregion

        #region Leaderboard Headers

        public static LeaderboardHeaders ReadLeaderboardHeaders()
        {
            var response = JsonConvert.DeserializeObject<LeaderboardHeadersResponse>(Resources.LeaderboardHeaders);

            return response.Leaderboards;
        }

        [DataContract]
        sealed class LeaderboardHeadersResponse
        {
            [DataMember(Name = "leaderboards")]
            public LeaderboardHeaders Leaderboards { get; } = new LeaderboardHeaders();
        }

        #endregion

        #region Daily Leaderboard Headers

        public static DailyLeaderboardHeaders ReadDailyLeaderboardHeaders()
        {
            var response = JsonConvert.DeserializeObject<DailyLeaderboardHeadersResponse>(Resources.DailyLeaderboardHeaders);

            return response.Leaderboards;
        }

        [DataContract]
        sealed class DailyLeaderboardHeadersResponse
        {
            [DataMember(Name = "leaderboards")]
            public DailyLeaderboardHeaders Leaderboards { get; } = new DailyLeaderboardHeaders();
        }

        #endregion
    }
}
