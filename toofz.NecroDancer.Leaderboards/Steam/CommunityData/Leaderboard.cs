using System.Xml.Serialization;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    [XmlRoot("leaderboard")]
    public sealed class Leaderboard
    {
        [XmlElement("url")]
        public string Url { get; set; }
        [XmlElement("lbid")]
        public int LeaderboardId { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("display_name")]
        public string DisplayName { get; set; }
        [XmlElement("entries")]
        public int EntryCount { get; set; }
        [XmlElement("sortmethod")]
        public int SortMethod { get; set; }
        [XmlElement("displaytype")]
        public int DisplayType { get; set; }
    }
}
