using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Properties;

namespace toofz.NecroDancer.Leaderboards
{
    public static class LeaderboardsResources
    {
        public static LeaderboardHeaders ReadLeaderboardHeaders()
        {
            var leaderboardHeadersEnvelope = JsonConvert.DeserializeObject<LeaderboardHeadersEnvelope>(Resources.LeaderboardHeaders);

            return leaderboardHeadersEnvelope.Leaderboards;
        }

        public static DailyLeaderboardHeaders ReadDailyLeaderboardHeaders()
        {
            var dailyLeaderboardHeadersEnvelope = JsonConvert.DeserializeObject<DailyLeaderboardHeadersEnvelope>(Resources.DailyLeaderboardHeaders);

            return dailyLeaderboardHeadersEnvelope.Leaderboards;
        }
    }
}
