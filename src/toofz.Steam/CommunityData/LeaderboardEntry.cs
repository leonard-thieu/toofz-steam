using System.Xml.Serialization;

namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("entry")]
    public sealed class LeaderboardEntry
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("steamid")]
        public long SteamId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("score")]
        public int Score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("rank")]
        public int Rank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("ugcid")]
        public ulong UgcId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("details")]
        public string Details { get; set; }
    }
}
