using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class LeaderboardHeader
    {
        [DataMember(Name = "id", IsRequired = true)]
        public int Id { get; set; }
        [DataMember(Name = "display_name", IsRequired = true)]
        public string DisplayName { get; set; }
        [DataMember(Name = "product", IsRequired = true)]
        public string Product { get; set; }
        [DataMember(Name = "mode", IsRequired = true)]
        public string Mode { get; set; }
        [DataMember(Name = "run", IsRequired = true)]
        public string Run { get; set; }
        [DataMember(Name = "character", IsRequired = true)]
        public string Character { get; set; }
    }
}
