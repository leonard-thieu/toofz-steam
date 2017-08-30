using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser
{
    public sealed class PlayerSummariesEnvelope
    {
        [DataMember(Name = "response")]
        public PlayerSummaries Response { get; set; }
    }
}
