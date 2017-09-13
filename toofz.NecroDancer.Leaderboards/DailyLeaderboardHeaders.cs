using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class DailyLeaderboardHeaders : Collection<DailyLeaderboardHeader> { }
}
