using System.Runtime.Serialization;

namespace toofz.Steam.WebApi.ISteamUser
{
    /// <summary>
    /// Basic profile information.
    /// </summary>
    [DataContract]
    public sealed class PlayerSummary
    {
        /// <summary>
        /// 64-bit SteamID of the user
        /// </summary>
        [DataMember(Name = "steamid", IsRequired = true)]
        public long SteamId { get; set; }
        /// <summary>
        /// This represents whether the profile is visible or not, and if it is visible, why you are allowed to see it. 
        /// Note that because this WebAPI does not use authentication, there are only two possible values returned: 
        ///   1 - The profile is not visible to you (Private, Friends Only, etc).
        ///   3 - The profile is "Public", and the data is visible.
        /// Mike Blaszczak's post on Steam forums says, 
        ///   The community visibility state this API returns is different than the privacy state. It's the effective visibility state from 
        ///   the account making the request to the account being viewed given the requesting account's relationship to the viewed account.
        /// </summary>
        [DataMember(Name = "communityvisibilitystate", IsRequired = true)]
        public int CommunityVisibilityState { get; set; }
        /// <summary>
        /// If set, indicates the user has a community profile configured (will be set to '1')
        /// </summary>
        [DataMember(Name = "profilestate")]
        public int ProfileState { get; set; }
        /// <summary>
        /// The player's persona name (display name)
        /// </summary>
        [DataMember(Name = "personaname", IsRequired = true)]
        public string PersonaName { get; set; }
        /// <summary>
        /// The last time the user was online, in unix time.
        /// </summary>
        [DataMember(Name = "lastlogoff", IsRequired = true)]
        public int LastLogOff { get; set; }
        /// <summary>
        /// The full URL of the player's Steam Community profile.
        /// </summary>
        [DataMember(Name = "profileurl", IsRequired = true)]
        public string ProfileUrl { get; set; }
        /// <summary>
        /// The full URL of the player's 32x32px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
        /// </summary>
        [DataMember(Name = "avatar", IsRequired = true)]
        public string Avatar { get; set; }
        /// <summary>
        /// The full URL of the player's 64x64px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
        /// </summary>
        [DataMember(Name = "avatarmedium", IsRequired = true)]
        public string AvatarMedium { get; set; }
        /// <summary>
        /// The full URL of the player's 184x184px avatar. If the user hasn't configured an avatar, this will be the default ? avatar.
        /// </summary>
        [DataMember(Name = "avatarfull", IsRequired = true)]
        public string AvatarFull { get; set; }
        /// <summary>
        /// The user's current status.
        ///   0 - Offline
        ///   1 - Online
        ///   2 - Busy
        ///   3 - Away
        ///   4 - Snooze
        ///   5 - Looking to Trade
        ///   6 - Looking to Play
        /// If the player's profile is private, this will always be "0", except if the user has set their status to Looking to Trade or Looking to Play, 
        /// because a bug makes those status appear even if the profile is private.
        /// </summary>
        [DataMember(Name = "personastate", IsRequired = true)]
        public int PersonaState { get; set; }
        /// <summary>
        /// The player's "Real Name", if they have set it.
        /// </summary>
        [DataMember(Name = "realname")]
        public string RealName { get; set; }
        /// <summary>
        /// The player's primary group, as configured in their Steam Community profile.
        /// </summary>
        [DataMember(Name = "primaryclanid")]
        public ulong PrimaryClanId { get; set; }
        /// <summary>
        /// The time the player's account was created.
        /// </summary>
        [DataMember(Name = "timecreated")]
        public int TimeCreated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "personastateflags")]
        public int PersonaStateFlags { get; set; }
        /// <summary>
        /// If set on the user's Steam Community profile, The user's country of residence, 2-character ISO country code
        /// </summary>
        [DataMember(Name = "loccountrycode")]
        public string LocCountryCode { get; set; }
        /// <summary>
        /// If set on the user's Steam Community profile, The user's state of residence
        /// </summary>
        [DataMember(Name = "locstatecode")]
        public string LocStateCode { get; set; }
        /// <summary>
        /// An internal code indicating the user's city of residence. A future update will provide this data in a more useful way.
        /// </summary>
        [DataMember(Name = "loccityid")]
        public int LocCityId { get; set; }
    }
}
