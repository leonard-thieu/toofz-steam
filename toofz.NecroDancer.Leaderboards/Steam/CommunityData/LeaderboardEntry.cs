using System.Xml.Serialization;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    [XmlRoot("entry")]
    public sealed class LeaderboardEntry
    {
        [XmlElement("steamid")]
        public long SteamId { get; set; }
        [XmlElement("score")]
        public int Score { get; set; }
        [XmlElement("rank")]
        public int Rank { get; set; }
        [XmlElement("ugcid")]
        public ulong UgcId { get; set; }
        [XmlElement("details")]
        public string Details { get; set; }
    }
}
