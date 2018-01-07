using System.Collections.Generic;
using System.Xml.Serialization;

namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("response")]
    public sealed class LeaderboardsEnvelope
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
        [XmlElement("leaderboardCount")]
        public int LeaderboardCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("leaderboard")]
        public List<Leaderboard> Leaderboards { get; set; }
    }
}
