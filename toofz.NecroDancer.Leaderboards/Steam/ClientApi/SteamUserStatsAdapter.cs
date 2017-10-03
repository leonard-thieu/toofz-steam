using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    [ExcludeFromCodeCoverage]
    sealed class SteamUserStatsAdapter : ISteamUserStats
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamUserStatsAdapter"/> class.
        /// </summary>
        /// <param name="steamUserStats">
        /// The <see cref="SteamUserStats"/> instance to wrap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="steamUserStats"/> is null.
        /// </exception>
        public SteamUserStatsAdapter(SteamUserStats steamUserStats)
        {
            this.steamUserStats = steamUserStats ?? throw new ArgumentNullException(nameof(steamUserStats), $"{nameof(steamUserStats)} is null.");
        }

        readonly SteamUserStats steamUserStats;

        /// <summary>
        /// Gets or sets the period of time before jobs will be considered timed out and will be canceled. 
        /// By default this is 10 seconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Asks the Steam back-end for a leaderboard by name for a given appid. Results
        /// are returned in a <see cref="IFindOrCreateLeaderboardCallback"/>.
        /// </summary>
        /// <param name="appId">The AppID to request a leaderboard for.</param>
        /// <param name="name">Name of the leaderboard to request.</param>
        public async Task<IFindOrCreateLeaderboardCallback> FindLeaderboard(uint appId, string name)
        {
            var asyncJob = steamUserStats.FindLeaderboard(appId, name);
            asyncJob.Timeout = Timeout;

            var leaderboard = await asyncJob.ToTask().ConfigureAwait(false);

            return new FindOrCreateLeaderboardCallbackAdapter(leaderboard);
        }

        /// <summary>
        /// Asks the Steam back-end for a set of rows in the leaderboard. Results are returned
        /// in a <see cref="ILeaderboardEntriesCallback"/>.
        /// </summary>
        /// <param name="appId">The AppID to request leaderboard rows for.</param>
        /// <param name="id">ID of the leaderboard to view.</param>
        /// <param name="rangeStart">Range start or 0.</param>
        /// <param name="rangeEnd">Range end or max leaderboard entries.</param>
        /// <param name="dataRequest">Type of request.</param>
        public async Task<ILeaderboardEntriesCallback> GetLeaderboardEntries(uint appId, int id, int rangeStart, int rangeEnd, ELeaderboardDataRequest dataRequest)
        {
            var asyncJob = steamUserStats.GetLeaderboardEntries(appId, id, rangeStart, rangeEnd, dataRequest);
            asyncJob.Timeout = Timeout;

            var leaderboardEntries = await asyncJob.ToTask().ConfigureAwait(false);

            return new LeaderboardEntriesCallbackAdapter(leaderboardEntries);
        }
    }
}
