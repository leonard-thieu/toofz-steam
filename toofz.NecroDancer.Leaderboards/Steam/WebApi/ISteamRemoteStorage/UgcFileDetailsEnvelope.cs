using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage
{
    [DataContract]
    public sealed class UgcFileDetailsEnvelope
    {
        /// <summary>
        /// UGC file information.
        /// </summary>
        [DataMember(Name = "data", IsRequired = true)]
        public UgcFileDetails Data { get; set; }
    }
}
