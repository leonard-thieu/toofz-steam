using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    [DataContract]
    public sealed class ReplayDTO
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }
        [DataMember(Name = "error", IsRequired = true)]
        public int? Error { get; set; }
        [DataMember(Name = "seed", IsRequired = true)]
        public int? Seed { get; set; }
        [DataMember(Name = "version", IsRequired = true)]
        public int? Version { get; set; }
        [DataMember(Name = "killed_by", IsRequired = true)]
        public string KilledBy { get; set; }
    }
}
