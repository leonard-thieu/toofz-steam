namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    /// <summary>
    /// Settings for <see cref="SteamCommunityDataClient"/>.
    /// </summary>
    public sealed class SteamCommunityDataClientSettings
    {
        /// <summary>
        /// If true, enables cache busting by appending a query parameter with a random value to requests; otherwise, false.
        /// The default is false.
        /// </summary>
        public bool IsCacheBustingEnabled { get; set; }
    }
}
