using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    sealed class DailyLeaderboardHeadersEnvelope
    {
        [DataMember(Name = "leaderboards", IsRequired = true)]
        public DailyLeaderboardHeaders Leaderboards { get; set; }
    }
}
