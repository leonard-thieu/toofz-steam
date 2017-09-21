using System;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    /// <summary>
    /// A Steam player.
    /// </summary>
    [DataContract]
    public sealed class PlayerDTO
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }
        /// <summary>
        /// The player's display name.
        /// </summary>
        [DataMember(Name = "display_name", IsRequired = true)]
        public string DisplayName { get; set; }
        /// <summary>
        /// The time (in UTC) that the player's data was retrieved at.
        /// </summary>
        [DataMember(Name = "updated_at", IsRequired = true)]
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// The URL of the player's avatar.
        /// </summary>
        [DataMember(Name = "avatar", IsRequired = true)]
        public string Avatar { get; set; }
    }
}
