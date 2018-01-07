using System.Collections.Generic;
using System.Xml.Serialization;

namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("response")]
    public sealed class LeaderboardEntriesEnvelope
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("appID")]
        public uint AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("appFriendlyName")]
        public string AppFriendlyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("leaderboardID")]
        public int LeaderboardId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("totalLeaderboardEntries")]
        public int TotalLeaderboardEntries { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("entryStart")]
        public int EntryStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("entryEnd")]
        public int EntryEnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("nextRequestURL")]
        public string NextRequestUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("resultCount")]
        public int ResultCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlArray("entries")]
        [XmlArrayItem("entry")]
        public List<LeaderboardEntry> Entries { get; set; }
    }
}
