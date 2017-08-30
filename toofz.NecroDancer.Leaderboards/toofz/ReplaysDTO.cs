using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class ReplaysDTO
    {
        [DataMember(Name = "total")]
        public int Total { get; set; }
        [DataMember(Name = "replays")]
        public IEnumerable<ReplayDTO> Replays { get; set; } = new List<ReplayDTO>();
    }
}
