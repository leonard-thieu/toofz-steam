using System;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class DailyLeaderboardHeader
    {
        [DataMember(Name = "id", IsRequired = true)]
        public int Id { get; set; }
        [DataMember(Name = "product", IsRequired = true)]
        public string Product { get; set; }
        [DataMember(Name = "date", IsRequired = true)]
        public DateTime Date { get; set; }
        [DataMember(Name = "production")]
        public bool IsProduction { get; set; }
    }
}
