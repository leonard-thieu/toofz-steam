using System.Xml.Serialization;

namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("leaderboard")]
    public sealed class Leaderboard
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("url")]
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("lbid")]
        public int LeaderboardId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("entries")]
        public int EntryCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("sortmethod")]
        public int SortMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("displaytype")]
        public int DisplayType { get; set; }
    }
}
