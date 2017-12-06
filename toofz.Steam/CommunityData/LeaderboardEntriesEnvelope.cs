using System.Collections.Generic;
using System.Xml.Serialization;

namespace toofz.Steam.CommunityData
{
    [XmlRoot("response")]
    public sealed class LeaderboardEntriesEnvelope
    {
        [XmlElement("appID")]
        public uint AppId { get; set; }
        [XmlElement("appFriendlyName")]
        public string AppFriendlyName { get; set; }
        [XmlElement("leaderboardID")]
        public int LeaderboardId { get; set; }
        [XmlElement("totalLeaderboardEntries")]
        public int TotalLeaderboardEntries { get; set; }
        [XmlElement("entryStart")]
        public int EntryStart { get; set; }
        [XmlElement("entryEnd")]
        public int EntryEnd { get; set; }
        [XmlElement("nextRequestURL")]
        public string NextRequestUrl { get; set; }
        [XmlElement("resultCount")]
        public int ResultCount { get; set; }
        [XmlArray("entries")]
        [XmlArrayItem("entry")]
        public List<LeaderboardEntry> Entries { get; set; }
    }
}
