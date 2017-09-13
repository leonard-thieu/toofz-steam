using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class LeaderboardHeader
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
        [DataMember(Name = "product")]
        public string Product { get; set; }
        [DataMember(Name = "mode")]
        public string Mode { get; set; }
        [DataMember(Name = "run")]
        public string Run { get; set; }
        [DataMember(Name = "character")]
        public string Character { get; set; }
    }
}
