using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam.CommunityData;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    public class SteamCommunityDataClientTests
    {
        public SteamCommunityDataClientTests()
        {
            steamCommunityDataClient = new SteamCommunityDataClient(handler, telemetryClient);
        }

        private MockHttpMessageHandler handler = new MockHttpMessageHandler();
        private TelemetryClient telemetryClient = new TelemetryClient();
        private SteamCommunityDataClient steamCommunityDataClient;

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var telemetryClient = new TelemetryClient();

                // Act
                var client = new SteamCommunityDataClient(handler, telemetryClient);

                // Assert
                Assert.IsAssignableFrom<SteamCommunityDataClient>(client);
            }
        }

        public class GetLeaderboardsAsyncMethod_UInt32 : SteamCommunityDataClientTests
        {
            [Fact]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/?xml=1")
                    .Respond(new StringContent(Resources.Leaderboards, Encoding.UTF8, "text/xml"));
                var appId = 247080U;

                // Act
                var leaderboardsEnvelope = await steamCommunityDataClient.GetLeaderboardsAsync(appId);

                // Assert
                Assert.Equal(411, leaderboardsEnvelope.Leaderboards.Count);
                var leaderboard = leaderboardsEnvelope.Leaderboards.First();
                Assert.Equal("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1", leaderboard.Url);
                Assert.Equal(2047387, leaderboard.LeaderboardId);
                Assert.Equal("DLC HARDCORE All Chars DLC_PROD", leaderboard.Name);
                Assert.Equal("All Characters (DLC) Score (Amplified)", leaderboard.DisplayName);
                Assert.Equal(317, leaderboard.EntryCount);
                Assert.Equal(2, leaderboard.SortMethod);
                Assert.Equal(1, leaderboard.DisplayType);
            }
        }

        public class GetLeaderboardsAsyncMethod_String : SteamCommunityDataClientTests
        {
            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                steamCommunityDataClient.Dispose();
                var communityGameName = 247080U.ToString();

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return steamCommunityDataClient.GetLeaderboardsAsync(communityGameName);
                });
            }

            [Fact]
            public async Task CommunityGameNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string communityGameName = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return steamCommunityDataClient.GetLeaderboardsAsync(communityGameName);
                });
            }

            [Fact]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/?xml=1")
                    .Respond(new StringContent(Resources.Leaderboards, Encoding.UTF8, "text/xml"));
                var communityGameName = 247080U.ToString();

                // Act
                var leaderboardsEnvelope = await steamCommunityDataClient.GetLeaderboardsAsync(communityGameName);

                // Assert
                Assert.Equal(411, leaderboardsEnvelope.Leaderboards.Count);
                var leaderboard = leaderboardsEnvelope.Leaderboards.First();
                Assert.Equal("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1", leaderboard.Url);
                Assert.Equal(2047387, leaderboard.LeaderboardId);
                Assert.Equal("DLC HARDCORE All Chars DLC_PROD", leaderboard.Name);
                Assert.Equal("All Characters (DLC) Score (Amplified)", leaderboard.DisplayName);
                Assert.Equal(317, leaderboard.EntryCount);
                Assert.Equal(2, leaderboard.SortMethod);
                Assert.Equal(1, leaderboard.DisplayType);
            }
        }

        public class GetLeaderboardEntriesAsyncMethod_UInt32 : SteamCommunityDataClientTests
        {
            [Fact]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(Resources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var appId = 247080U;
                var leaderboardId = 2047387;

                // Act
                var entries = await steamCommunityDataClient.GetLeaderboardEntriesAsync(appId, leaderboardId);

                // Assert
                Assert.Equal(317, entries.Count());
                var entry = entries.First();
                Assert.Equal(76561197998799529, entry.SteamId);
                Assert.Equal(134377, entry.Score);
                Assert.Equal(1, entry.Rank);
                Assert.Equal(849347241492683863L, entry.ReplayId);
                Assert.Equal(11, entry.Zone);
                Assert.Equal(1, entry.Level);
            }
        }

        public class GetLeaderboardEntriesAsyncMethod_String : SteamCommunityDataClientTests
        {
            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                steamCommunityDataClient.Dispose();
                var communityGameName = 247080U.ToString();
                var leaderboardId = 2047387;

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return steamCommunityDataClient.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);
                });
            }

            [Fact]
            public async Task CommunityGameNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string communityGameName = null;
                var leaderboardId = 2047387;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return steamCommunityDataClient.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);
                });
            }

            [Fact]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(Resources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var communityGameName = 247080U.ToString();
                var leaderboardId = 2047387;

                // Act
                var entries = await steamCommunityDataClient.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);

                // Assert
                Assert.Equal(317, entries.Count());
                var entry = entries.First();
                Assert.Equal(76561197998799529, entry.SteamId);
                Assert.Equal(134377, entry.Score);
                Assert.Equal(1, entry.Rank);
                Assert.Equal(849347241492683863L, entry.ReplayId);
                Assert.Equal(11, entry.Zone);
                Assert.Equal(1, entry.Level);
            }
        }

        public class DisposeMethod
        {
            private SimpleHttpMessageHandler handler = new SimpleHttpMessageHandler();
            private TelemetryClient telemetryClient = new TelemetryClient();

            [Fact]
            public void DisposesHttpClient()
            {
                // Arrange
                var client = new SteamCommunityDataClient(handler, telemetryClient);

                // Act
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }

            [Fact]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange
                var client = new SteamCommunityDataClient(handler, telemetryClient);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }
        }
    }
}
