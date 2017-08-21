namespace toofz.NecroDancer.Leaderboards.toofz
{
    public class Replay
    {
        public long id { get; set; }
        public int? error { get; set; }
        public int? seed { get; set; }
        public int? version { get; set; }
        public string killed_by { get; set; }
    }
}
