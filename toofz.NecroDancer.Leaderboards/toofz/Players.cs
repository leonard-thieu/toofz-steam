using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    /// <summary>
    /// A page of Steam players.
    /// </summary>
    public class Players
    {
        /// <summary>
        /// Total number of players.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of players.
        /// </summary>
        public IEnumerable<Player> players { get; set; } = new List<Player>();
    }
}
