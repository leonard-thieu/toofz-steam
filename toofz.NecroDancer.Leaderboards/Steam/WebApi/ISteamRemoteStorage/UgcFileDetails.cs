using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage
{
    public sealed class UgcFileDetails
    {
        /// <summary>
        /// UGC file information.
        /// </summary>
        [JsonProperty("data")]
        public UgcFileDetailsData Data { get; set; }
    }
}
