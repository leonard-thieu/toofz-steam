using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            Client = new SteamCommunityDataClient(Handler);
        }

        public MockHttpMessageHandler Handler { get; set; } = new MockHttpMessageHandler();
        public SteamCommunityDataClient Client { get; set; }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();

                // Act
                var client = new SteamCommunityDataClient(handler);

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
                Handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/?xml=1")
                    .Respond(new StringContent(Resources.Leaderboards, Encoding.UTF8, "text/xml"));
                var appId = 247080U;

                // Act
                var leaderboardsEnvelope = await Client.GetLeaderboardsAsync(appId);

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
                Client.Dispose();
                var communityGameName = 247080U.ToString();

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetLeaderboardsAsync(communityGameName);
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
                    return Client.GetLeaderboardsAsync(communityGameName);
                });
            }

            [Fact]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                Handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/?xml=1")
                    .Respond(new StringContent(Resources.Leaderboards, Encoding.UTF8, "text/xml"));
                var communityGameName = 247080U.ToString();

                // Act
                var leaderboardsEnvelope = await Client.GetLeaderboardsAsync(communityGameName);

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
                Handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(Resources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var appId = 247080U;
                var leaderboardId = 2047387;

                // Act
                var leaderboardEntriesEnvelope = await Client.GetLeaderboardEntriesAsync(appId, leaderboardId);

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

        public class GetLeaderboardEntriesAsyncMethod_String : SteamCommunityDataClientTests
        {
            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                var communityGameName = 247080U.ToString();
                var leaderboardId = 2047387;

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);
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
                    return Client.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);
                });
            }

            [Fact]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                Handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(Resources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var communityGameName = 247080U.ToString();
                var leaderboardId = 2047387;

                // Act
                var leaderboardEntriesEnvelope = await Client.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);

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

        public class DisposeMethod
        {
            [Fact]
            public void DisposesHttpClient()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new SteamCommunityDataClient(handler);

                // Act
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }

            [Fact]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new SteamCommunityDataClient(handler);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }
        }
    }
}
