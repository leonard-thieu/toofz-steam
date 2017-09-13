using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    [DataContract]
    public sealed class ReplaysEnvelope
    {
        [DataMember(Name = "total")]
        public int Total { get; set; }
        [DataMember(Name = "replays")]
        public IEnumerable<ReplayDTO> Replays { get; set; } = new List<ReplayDTO>();
    }
}
