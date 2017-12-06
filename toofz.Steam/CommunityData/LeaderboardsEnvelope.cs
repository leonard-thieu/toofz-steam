using System.Collections.Generic;
using System.Xml.Serialization;

namespace toofz.Steam.CommunityData
{
    [XmlRoot("response")]
    public sealed class LeaderboardsEnvelope
    {
        [XmlElement("appID")]
        public uint AppId { get; set; }
        [XmlElement("appFriendlyName")]
        public string AppFriendlyName { get; set; }
        [XmlElement("leaderboardCount")]
        public int LeaderboardCount { get; set; }
        [XmlElement("leaderboard")]
        public List<Leaderboard> Leaderboards { get; set; }
    }
}
