using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer player.
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        public long SteamId { get; set; }
        /// <summary>
        /// Entries that the player has submitted.
        /// </summary>
        public List<Entry> Entries { get; } = new List<Entry>();
        /// <summary>
        /// Daily entries that the player has submitted.
        /// </summary>
        public List<DailyEntry> DailyEntries { get; } = new List<DailyEntry>();
        /// <summary>
        /// The last time the player's information was updated.
        /// </summary>
        public DateTime? LastUpdate { get; set; }
        /// <summary>
        /// Indicates if Steam returned information for the player.
        /// </summary>
        public bool? Exists { get; set; }
        /// <summary>
        /// The player's display name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The full URL of the player's 32x32px avatar.
        /// </summary>
        public string Avatar { get; set; }
    }
}
