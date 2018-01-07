using System.Runtime.Serialization;

namespace toofz.Steam.WebApi.ISteamUser
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public sealed class PlayerSummariesEnvelope
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "response", IsRequired = true)]
        public PlayerSummaries Response { get; set; }
    }
}
