using System.Runtime.Serialization;

namespace toofz.Steam.WebApi.ISteamUser
{
    [DataContract]
    public sealed class PlayerSummariesEnvelope
    {
        [DataMember(Name = "response", IsRequired = true)]
        public PlayerSummaries Response { get; set; }
    }
}
