using System;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    /// <summary>
    /// A Steam player.
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// The player's display name.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// The time (in UTC) that the player's data was retrieved at.
        /// </summary>
        public DateTime? updated_at { get; set; }
        /// <summary>
        /// The URL of the player's avatar.
        /// </summary>
        public string avatar { get; set; }
    }
}
