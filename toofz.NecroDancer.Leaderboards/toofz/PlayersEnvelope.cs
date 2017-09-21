using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    /// <summary>
    /// A page of Steam players.
    /// </summary>
    [DataContract]
    public sealed class PlayersEnvelope
    {
        /// <summary>
        /// Total number of players.
        /// </summary>
        [DataMember(Name = "total", IsRequired = true)]
        public int Total { get; set; }
        /// <summary>
        /// A collection of players.
        /// </summary>
        [DataMember(Name = "players", IsRequired = true)]
        public IEnumerable<PlayerDTO> Players { get; set; }
    }
}
