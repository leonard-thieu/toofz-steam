using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardHeader
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("run")]
        public string Run { get; set; }
        [JsonProperty("character")]
        public string Character { get; set; }
    }
}
