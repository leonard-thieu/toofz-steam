using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser
{
    public sealed class PlayerSummaries
    {
        [JsonProperty("response")]
        public PlayerSummariesResponse Response { get; set; }
    }
}
