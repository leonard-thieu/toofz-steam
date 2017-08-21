using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class DailyLeaderboardHeader
    {
        public int id { get; set; }
        public string product { get; set; }
        public bool production { get; set; }
        public DateTime date { get; set; }
    }
}
