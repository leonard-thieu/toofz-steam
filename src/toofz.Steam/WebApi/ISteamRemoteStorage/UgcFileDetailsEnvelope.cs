using System.Runtime.Serialization;

namespace toofz.Steam.WebApi.ISteamRemoteStorage
{
    /// <summary>
    /// 
    /// </summary>
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
