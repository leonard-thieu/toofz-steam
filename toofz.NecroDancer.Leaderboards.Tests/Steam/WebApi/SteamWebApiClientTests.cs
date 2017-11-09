using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
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
            client = new SteamWebApiClient(handler, telemetryClient);
        }

        private MockHttpMessageHandler handler = new MockHttpMessageHandler();
        private TelemetryClient telemetryClient = new TelemetryClient();
        private SteamWebApiClient client;

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var telemetryClient = new TelemetryClient();

                // Act
                var client = new SteamWebApiClient(handler, telemetryClient);

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
                client.Dispose();
                client.SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[0];

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                client.SteamWebApiKey = null;
                var steamIds = new long[0];

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task SteamIdsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                client.SteamWebApiKey = "mySteamWebApiKey";
                long[] steamIds = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task TooManySteamIds_ThrowsArgumentException()
            {
                // Arrange
                client.SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[SteamWebApiClient.MaxPlayerSummariesPerRequest + 1];

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentException>(() =>
                {
                    return client.GetPlayerSummariesAsync(steamIds);
                });
            }

            [Fact]
            public async Task ReturnsPlayerSummaries()
            {
                // Arrange
                client.SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[] { 76561197960435530 };
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", client.SteamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond("application/json", Resources.PlayerSummariesEnvelope);

                // Act
                var playerSummariesEnvelope = await client.GetPlayerSummariesAsync(steamIds);

                // Assert
                Assert.IsAssignableFrom<PlayerSummariesEnvelope>(playerSummariesEnvelope);
            }

            [Fact]
            public async Task ResponseContainsâ_DoesNotThrowDecoderFallbackException()
            {
                // Arrange
                client.SteamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[] { 76561197960435530 };
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", client.SteamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond(new StringContent(Resources.StarWarsEncoding, Encoding.Default, "application/json"));

                // Act
                var playerSummariesEnvelope = await client.GetPlayerSummariesAsync(steamIds);

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
                client.Dispose();
                client.SteamWebApiKey = "mySteamWebApiKey";
                var appId = 247080U;
                var ugcId = 22837952671856412;

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return client.GetUgcFileDetailsAsync(appId, ugcId);
                });
            }

            [Fact]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                client.SteamWebApiKey = null;
                var appId = 247080U;
                var ugcId = 22837952671856412;

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return client.GetUgcFileDetailsAsync(appId, ugcId);
                });
            }

            [Fact]
            public async Task ReturnsUgcFileDetails()
            {
                // Arrange
                client.SteamWebApiKey = "mySteamWebApiKey";
                var appId = 247080U;
                var ugcId = 22837952671856412;
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamRemoteStorage/GetUGCFileDetails/v1")
                    .WithQueryString("key", client.SteamWebApiKey)
                    .WithQueryString("appid", appId.ToString())
                    .WithQueryString("ugcid", ugcId.ToString())
                    .Respond("application/json", Resources.UgcFileDetailsEnvelope);

                // Act
                var ugcFileDetailsEnvelope = await client.GetUgcFileDetailsAsync(appId, ugcId);

                // Assert
                Assert.IsAssignableFrom<UgcFileDetailsEnvelope>(ugcFileDetailsEnvelope);
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
                var client = new SteamWebApiClient(handler, telemetryClient);

                // Act
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }

            [Fact]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange
                var client = new SteamWebApiClient(handler, telemetryClient);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }
        }
    }
}
