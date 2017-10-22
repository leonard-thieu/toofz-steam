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
        /// The leaderboard's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The leaderboard's display name.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Indicates if the leaderboard is a production leaderboard.
        /// </summary>
        public bool IsProduction { get; set; }
        /// <summary>
        /// The product associated with the leaderboard.
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// The ID of the product associated with the leaderboard.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// The mode associated with the leaderboard.
        /// </summary>
        public Mode Mode { get; set; }
        /// <summary>
        /// The ID of the mode associated with the leaderboard.
        /// </summary>
        public int ModeId { get; set; }
        /// <summary>
        /// The run associated with the leaderboard.
        /// </summary>
        public Run Run { get; set; }
        /// <summary>
        /// The ID of the run associated with the leaderboard.
        /// </summary>
        public int RunId { get; set; }
        /// <summary>
        /// The character associated with the leaderboard.
        /// </summary>
        public Character Character { get; set; }
        /// <summary>
        /// The ID of the character associated with the leaderboard.
        /// </summary>
        public int CharacterId { get; set; }
        /// <summary>
        /// Indicates if the leaderboard is a Co-op leaderboard.
        /// </summary>
        public bool IsCoOp { get; set; }
        /// <summary>
        /// Indicates if the leaderboard is a Custom Music leaderboard.
        /// </summary>
        public bool IsCustomMusic { get; set; }
    }
}
