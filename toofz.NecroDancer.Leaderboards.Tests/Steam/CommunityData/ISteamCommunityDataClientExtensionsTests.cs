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
    public class ISteamCommunityDataClientExtensionsTests
    {
        public class GetLeaderboardsAsyncMethod
        {
            [Fact]
            public async Task SteamCommunityDataClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ISteamCommunityDataClient steamCommunityDataClient = null;
                var appId = 247080U;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return steamCommunityDataClient.GetLeaderboardsAsync(appId);
                });
            }

            [Fact]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var telemetryClient = new TelemetryClient();
                var steamCommunityDataClient = new SteamCommunityDataClient(handler, telemetryClient);
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

        public class GetLeaderboardEntriesAsyncMethod
        {
            [Fact]
            public void ClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ISteamCommunityDataClient client = null;
                var appId = 247080U;
                var leaderboardId = 739999;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    client.GetLeaderboardEntriesAsync(appId, leaderboardId);
                });
            }

            [Fact]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var telemetryClient = new TelemetryClient();
                var steamCommunityDataClient = new SteamCommunityDataClient(handler, telemetryClient);
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(Resources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var appId = 247080U;
                var leaderboardId = 2047387;

                // Act
                var leaderboardEntriesEnvelope = await steamCommunityDataClient.GetLeaderboardEntriesAsync(appId, leaderboardId);

                // Assert
                var entries = leaderboardEntriesEnvelope.Entries;
                Assert.Equal(317, entries.Count);
                var entry = entries.First();
                Assert.Equal(76561197998799529, entry.SteamId);
                Assert.Equal(134377, entry.Score);
                Assert.Equal(1, entry.Rank);
                Assert.Equal(849347241492683863UL, entry.UgcId);
                Assert.Equal("0b00000001000000", entry.Details);
            }
        }
    }
}
