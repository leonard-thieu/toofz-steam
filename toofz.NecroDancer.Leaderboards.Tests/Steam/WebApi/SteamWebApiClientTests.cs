using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    public class SteamWebApiClientTests
    {
        public SteamWebApiClientTests()
        {
            Client = new SteamWebApiClient(Handler);
        }

        public MockHttpMessageHandler Handler { get; set; } = new MockHttpMessageHandler();
        public SteamWebApiClient Client { get; set; }
        public string SteamWebApiKey
        {
            get { return Client.SteamWebApiKey; }
            set { Client.SteamWebApiKey = value; }
        }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();

                // Act
                var client = new SteamWebApiClient(handler);

                // Assert
                Assert.IsAssignableFrom<SteamWebApiClient>(client);
            }
        }

        public class GetPlayerSummariesAsync : SteamWebApiClientTests
        {
            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[0];

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                SteamWebApiKey = null;
                var steamIds = new long[0];

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return Client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task SteamIdsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                SteamWebApiKey = "mySteamWebApiKey";
                long[] steamIds = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return Client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task TooManySteamIds_ThrowsArgumentException()
            {
                // Arrange
                SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[SteamWebApiClient.MaxPlayerSummariesPerRequest + 1];

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentException>(() =>
                {
                    return Client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task ReturnsPlayerSummaries()
            {
                // Arrange
                SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[] { 76561197960435530 };
                Handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", SteamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond("application/json", Resources.PlayerSummariesEnvelope);

                // Act
                var playerSummariesEnvelope = await Client.GetPlayerSummariesAsync(steamIds);

                // Assert
                Assert.IsAssignableFrom<PlayerSummariesEnvelope>(playerSummariesEnvelope);
            }

            [Fact]
            public async Task ResponseContainsâ_DoesNotThrowDecoderFallbackException()
            {
                // Arrange
                SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[] { 76561197960435530 };
                Handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", SteamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond(new StringContent(Resources.StarWarsEncoding, Encoding.Default, "application/json"));

                // Act
                var playerSummariesEnvelope = await Client.GetPlayerSummariesAsync(steamIds);

                // Assert
                Assert.IsAssignableFrom<PlayerSummariesEnvelope>(playerSummariesEnvelope);
            }
        }

        public class GetUgcFileDetailsAsync : SteamWebApiClientTests
        {
            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                SteamWebApiKey = "mySteamWebApiKey";
                var appId = 247080U;
                var ugcId = 22837952671856412;

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetUgcFileDetailsAsync(appId, ugcId);
                });
            }

            [Fact]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                SteamWebApiKey = null;
                var appId = 247080U;
                var ugcId = 22837952671856412;

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return Client.GetUgcFileDetailsAsync(appId, ugcId);
                });
            }

            [Fact]
            public async Task ReturnsUgcFileDetails()
            {
                // Arrange
                SteamWebApiKey = "mySteamWebApiKey";
                var appId = 247080U;
                var ugcId = 22837952671856412;
                Handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamRemoteStorage/GetUGCFileDetails/v1")
                    .WithQueryString("key", SteamWebApiKey)
                    .WithQueryString("appid", appId.ToString())
                    .WithQueryString("ugcid", ugcId.ToString())
                    .Respond("application/json", Resources.UgcFileDetailsEnvelope);

                // Act
                var ugcFileDetailsEnvelope = await Client.GetUgcFileDetailsAsync(appId, ugcId);

                // Assert
                Assert.IsAssignableFrom<UgcFileDetailsEnvelope>(ugcFileDetailsEnvelope);
            }
        }

        public class DisposeMethod
        {
            [Fact]
            public void DisposesHttpClient()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new SteamWebApiClient(handler);

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
                var client = new SteamWebApiClient(handler);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }
        }
    }
}
