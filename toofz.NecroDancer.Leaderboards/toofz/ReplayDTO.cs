using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    [DataContract]
    public sealed class ReplayDTO
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
        [DataMember(Name = "error")]
        public int? Error { get; set; }
        [DataMember(Name = "seed")]
        public int? Seed { get; set; }
        [DataMember(Name = "version")]
        public int? Version { get; set; }
        [DataMember(Name = "killed_by")]
        public string KilledBy { get; set; }
    }
}
