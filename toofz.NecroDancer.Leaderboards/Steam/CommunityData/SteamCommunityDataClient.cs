using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Flurl;
using Microsoft.ApplicationInsights;
using Polly;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    public sealed class SteamCommunityDataClient : ISteamCommunityDataClient
    {
        private static readonly XmlSerializer LeaderboardsEnvelopeSerializer = XmlSerializer.FromTypes(new[] { typeof(LeaderboardsEnvelope) })[0];
        private static readonly XmlSerializer LeaderboardEntriesEnvelopeSerializer = XmlSerializer.FromTypes(new[] { typeof(LeaderboardEntriesEnvelope) })[0];

        /// <summary>
        /// Gets a retry strategy for <see cref="SteamCommunityDataClient"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="PolicyBuilder"/> configured with a retry strategy appropriate for <see cref="SteamCommunityDataClient"/>.
        /// </returns>
        public static PolicyBuilder GetRetryStrategy()
        {
            return Policy
                .Handle<HttpRequestStatusException>(ex =>
                {
                    switch ((int)ex.StatusCode)
                    {
                        case 408:   // Request Timeout
                        case 429:   // Too Many Requests
                        case 500:   // Internal Server Error
                        case 502:   // Bad Gateway
                        case 503:   // Service Unavailable
                        case 504:   // Gateway Timeout
                            return true;
                        default:
                            return false;
                    }
                });
        }

        /// <summary>
        /// Initializes an instance of the <see cref="SteamCommunityDataClient"/> class with a specific handler.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="telemetryClient">The telemetry client to use for reporting telemetry.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public SteamCommunityDataClient(HttpMessageHandler handler, TelemetryClient telemetryClient) : this(handler, telemetryClient, default) { }

        /// <summary>
        /// Initializes an instance of the <see cref="SteamCommunityDataClient"/> class with a specific handler and settings.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="telemetryClient">The telemetry client to use for reporting telemetry.</param>
        /// <param name="settings">The settings to apply to <see cref="SteamCommunityDataClient"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public SteamCommunityDataClient(HttpMessageHandler handler, TelemetryClient telemetryClient, SteamCommunityDataClientSettings settings)
        {
            http = new ProgressReporterHttpClient(handler, true, telemetryClient) { BaseAddress = new Uri("http://steamcommunity.com/") };

            settings = settings ?? new SteamCommunityDataClientSettings();
            isCacheBustingEnabled = settings.IsCacheBustingEnabled;
        }

        private readonly ProgressReporterHttpClient http;
        private readonly bool isCacheBustingEnabled;

        #region GetLeaderboards

        public async Task<LeaderboardsEnvelope> GetLeaderboardsAsync(
            string communityGameName,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SteamCommunityDataClient));
            if (communityGameName == null)
                throw new ArgumentNullException(nameof(communityGameName));

            var requestUri = $"stats/{communityGameName}/leaderboards/"
                .SetQueryParams(new
                {
                    xml = 1,
                    v = GetRandomCacheBustingValue(),
                });
            var response = await http.GetAsync("Get leaderboards", requestUri, progress, cancellationToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return (LeaderboardsEnvelope)LeaderboardsEnvelopeSerializer.Deserialize(content);
        }

        #endregion

        #region GetLeaderboardEntries

        /// <summary>
        /// The maximum number of leaderboard entries allowed per request.
        /// </summary>
        public const int MaxLeaderboardEntriesPerRequest = 5001;

        public async Task<LeaderboardEntriesEnvelope> GetLeaderboardEntriesAsync(
            string communityGameName,
            int leaderboardId,
            GetLeaderboardEntriesParams @params = default,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SteamCommunityDataClient));
            if (communityGameName == null)
                throw new ArgumentNullException(nameof(communityGameName));

            @params = @params ?? new GetLeaderboardEntriesParams();

            var requestUri = $"stats/{communityGameName}/leaderboards/{leaderboardId}/"
                .SetQueryParams(new
                {
                    xml = 1,
                    start = @params.StartRange,
                    end = @params.EndRange,
                    v = GetRandomCacheBustingValue(),
                });
            var response = await http.GetAsync("Get leaderboard entries", requestUri, progress, cancellationToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return (LeaderboardEntriesEnvelope)LeaderboardEntriesEnvelopeSerializer.Deserialize(content);
        }

        #endregion

        #region GetRandomCacheBustingValue

        private static readonly char[] Base64Padding = { '=' };

        private string GetRandomCacheBustingValue()
        {
            if (!isCacheBustingEnabled) { return null; }

            var guid = Guid.NewGuid();

            return Convert.ToBase64String(guid.ToByteArray())
                .TrimEnd(Base64Padding)
                .Replace('+', '-')
                .Replace('/', '_');
        }

        #endregion

        #region IDisposable Implementation

        private bool disposed;

        /// <summary>
        /// Disposes of resources used by <see cref="SteamCommunityDataClient"/>.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }

            http.Dispose();

            disposed = true;
        }

        #endregion
    }
}
