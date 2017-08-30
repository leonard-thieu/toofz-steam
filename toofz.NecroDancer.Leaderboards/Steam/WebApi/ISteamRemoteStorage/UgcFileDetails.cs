using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage
{
    /// <summary>
    /// UGC file information.
    /// </summary>
    public sealed class UgcFileDetails
    {
        /// <summary>
        /// Path to the file along with its name
        /// </summary>
        [DataMember(Name = "filename")]
        public string FileName { get; set; }
        /// <summary>
        /// URL to the file
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }
        /// <summary>
        /// Size of the file
        /// </summary>
        [DataMember(Name = "size")]
        public int Size { get; set; }
    }
}
