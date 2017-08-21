namespace toofz.NecroDancer.Leaderboards.toofz
{
    public interface IPagination
    {
        int? Offset { get; set; }
        int? Limit { get; set; }
    }
}
