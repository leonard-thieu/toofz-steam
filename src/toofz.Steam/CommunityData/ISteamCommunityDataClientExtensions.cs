using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.Steam.CommunityData
{
    /// <summary>
    /// Contains extension methods for <see cref="ISteamCommunityDataClient"/>.
    /// </summary>
    public static class ISteamCommunityDataClientExtensions
    {
        public static Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            this ISteamCommunityDataClient steamCommunityDataClient,
            uint appId,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default)
        {
            if (steamCommunityDataClient == null)
                throw new ArgumentNullException(nameof(steamCommunityDataClient));

            return steamCommunityDataClient.GetLeaderboardsAsync(appId.ToString(), progress, cancellationToken);
        }

        public static Task<LeaderboardEntriesEnvelope> GetLeaderboardEntriesAsync(
            this ISteamCommunityDataClient steamCommunityDataClient,
            uint appId,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default)
        {
            if (steamCommunityDataClient == null)
                throw new ArgumentNullException(nameof(steamCommunityDataClient));

            return steamCommunityDataClient.GetLeaderboardEntriesAsync(appId.ToString(), leaderboardId, @params, progress, cancellationToken);
        }
    }
}
