using System.Collections.Generic;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser
{
    public sealed class PlayerSummariesResponse
    {
        /// <summary>
        /// A list of profile objects. Contained information varies depending on whether or not the user has their profile 
        /// set to Friends only or Private.
        /// </summary>
        [JsonProperty("players")]
        public List<PlayerSummary> Players { get; } = new List<PlayerSummary>();
    }
}
