using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer leaderboard.
    /// </summary>
    public sealed class Leaderboard
    {
        /// <summary>
        /// A value that uniquely identifies the leaderboard.
        /// </summary>
        public int LeaderboardId { get; set; }
        /// <summary>
        /// The leaderboard's collection of entries.
        /// </summary>
        public List<Entry> Entries { get; } = new List<Entry>();
        /// <summary>
        /// The last time that the leaderboard was updated.
        /// </summary>
        public DateTime? LastUpdate { get; set; }
        /// <summary>
        /// Indicates if the leaderboard is a production leaderboard.
        /// </summary>
        public bool IsProduction { get; set; }
        /// <summary>
        /// The ID of the product associated with the leaderboard.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// The ID of the mode associated with the leaderboard.
        /// </summary>
        public int ModeId { get; set; }
        /// <summary>
        /// The ID of the run associated with the leaderboard.
        /// </summary>
        public int RunId { get; set; }
        /// <summary>
        /// The ID of the character associated with the leaderboard.
        /// </summary>
        public int CharacterId { get; set; }
    }
}
