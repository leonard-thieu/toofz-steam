using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class LeaderboardHeaders : Collection<LeaderboardHeader> { }
}
