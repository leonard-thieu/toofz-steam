using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser
{
    [DataContract]
    public sealed class PlayerSummariesEnvelope
    {
        [DataMember(Name = "response")]
        public PlayerSummaries Response { get; set; }
    }
}
