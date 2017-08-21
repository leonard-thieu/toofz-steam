using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public class Replays
    {
        public int total { get; set; }
        public IEnumerable<Replay> replays { get; set; } = new List<Replay>();
    }
}
