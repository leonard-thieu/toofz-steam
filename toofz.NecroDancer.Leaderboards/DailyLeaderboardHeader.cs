using System;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class DailyLeaderboardHeader
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "product")]
        public string Product { get; set; }
        [DataMember(Name = "production")]
        public bool IsProduction { get; set; }
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
    }
}
