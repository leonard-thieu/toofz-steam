namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    /// <summary>
    /// Optional parameters for <see cref="ISteamCommunityDataClient.GetLeaderboardEntriesAsync(string, int, GetLeaderboardEntriesParams, System.IProgress{long}, System.Threading.CancellationToken)"/>.
    /// </summary>
    public sealed class GetLeaderboardEntriesParams
    {
        public int? StartRange { get; set; }
        public int? EndRange { get; set; }
    }
}
