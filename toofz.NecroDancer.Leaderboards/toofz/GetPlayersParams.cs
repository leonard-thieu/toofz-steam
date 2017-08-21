namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class GetPlayersParams : IPagination
    {
        public string Query { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string Sort { get; set; }
    }
}
