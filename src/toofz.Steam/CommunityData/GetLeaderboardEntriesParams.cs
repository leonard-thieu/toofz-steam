namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// Optional parameters for <see cref="ISteamCommunityDataClient.GetLeaderboardEntriesAsync(string, int, GetLeaderboardEntriesParams, System.IProgress{long}, System.Threading.CancellationToken)"/>.
    /// </summary>
    public sealed class GetLeaderboardEntriesParams
    {
        /// <summary>
        /// 
        /// </summary>
        public int? StartRange { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? EndRange { get; set; }
    }
}
