using System;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class DailyLeaderboardHeader
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("production")]
        public bool IsProduction { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
