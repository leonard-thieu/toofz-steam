using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    [DataContract]
    public sealed class ReplaysEnvelope
    {
        [DataMember(Name = "total", IsRequired = true)]
        public int Total { get; set; }
        [DataMember(Name = "replays", IsRequired = true)]
        public IEnumerable<ReplayDTO> Replays { get; set; }
    }
}
