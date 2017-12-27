using System.Runtime.Serialization;

namespace toofz.Steam.WebApi.ISteamRemoteStorage
{
    /// <summary>
    /// UGC file information.
    /// </summary>
    [DataContract]
    public sealed class UgcFileDetails
    {
        /// <summary>
        /// Path to the file along with its name
        /// </summary>
        [DataMember(Name = "filename", IsRequired = true)]
        public string FileName { get; set; }
        /// <summary>
        /// URL to the file
        /// </summary>
        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }
        /// <summary>
        /// Size of the file
        /// </summary>
        [DataMember(Name = "size", IsRequired = true)]
        public int Size { get; set; }
    }
}
