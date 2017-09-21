using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamUser;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    class SteamWebApiClientTests
    {
        [TestClass]
        public class GetPlayerSummariesAsync
        {
            [TestMethod]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                string steamWebApiKey = null;
                var handler = new MockHttpMessageHandler();
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };
                var steamIds = new long[0];

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                {
                    return steamWebApiClient.GetPlayerSummariesAsync(steamIds);
                });
            }

            [TestMethod]
            public async Task SteamIdsIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var steamWebApiKey = "mySteamWebApiKey";
                var handler = new MockHttpMessageHandler();
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };
                long[] steamIds = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return steamWebApiClient.GetPlayerSummariesAsync(steamIds);
                });
            }

            [TestMethod]
            public async Task TooManySteamIds_ThrowsArgumentException()
            {
                // Arrange
                var steamWebApiKey = "mySteamWebApiKey";
                var handler = new MockHttpMessageHandler();
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };
                var steamIds = new long[SteamWebApiClient.MaxPlayerSummariesPerRequest + 1];

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                {
                    return steamWebApiClient.GetPlayerSummariesAsync(steamIds);
                });
            }

            [TestMethod]
            public async Task ReturnsPlayerSummaries()
            {
                // Arrange
                var steamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[] { 76561197960435530 };
                var handler = new MockHttpMessageHandler();
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", steamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond("application/json", Resources.PlayerSummariesEnvelope);
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };

                // Act
                var playerSummariesEnvelope = await steamWebApiClient.GetPlayerSummariesAsync(steamIds);

                // Assert
                Assert.IsInstanceOfType(playerSummariesEnvelope, typeof(PlayerSummariesEnvelope));
            }

            [TestMethod]
            public async Task ResponseContainsâ_DoesNotThrowDecoderFallbackException()
            {
                // Arrange
                var steamWebApiKey = "mySteamWebApiKey";
                var steamIds = new long[] { 76561197960435530 };
                var handler = new MockHttpMessageHandler();
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", steamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond(new StringContent(Resources.StarWarsEncoding, Encoding.Default, "application/json"));
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };

                // Act
                var playerSummariesEnvelope = await steamWebApiClient.GetPlayerSummariesAsync(steamIds);

                // Assert
                Assert.IsInstanceOfType(playerSummariesEnvelope, typeof(PlayerSummariesEnvelope));
            }
        }

        [TestClass]
        public class GetUgcFileDetailsAsync
        {
            [TestMethod]
            public async Task SteamWebApiKeyIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                string steamWebApiKey = null;
                var handler = new MockHttpMessageHandler();
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };
                var appId = 247080U;
                var ugcId = 22837952671856412;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                {
                    return steamWebApiClient.GetUgcFileDetailsAsync(appId, ugcId);
                });
            }

            [TestMethod]
            public async Task ReturnsUgcFileDetails()
            {
                // Arrange
                var steamWebApiKey = "mySteamWebApiKey";
                var appId = 247080U;
                var ugcId = 22837952671856412;
                var handler = new MockHttpMessageHandler();
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamRemoteStorage/GetUGCFileDetails/v1")
                    .WithQueryString("key", steamWebApiKey)
                    .WithQueryString("appid", appId.ToString())
                    .WithQueryString("ugcid", ugcId.ToString())
                    .Respond("application/json", Resources.UgcFileDetailsEnvelope);
                var steamWebApiClient = new SteamWebApiClient(handler) { SteamWebApiKey = steamWebApiKey };

                // Act
                var ugcFileDetailsEnvelope = await steamWebApiClient.GetUgcFileDetailsAsync(appId, ugcId);

                // Assert
                Assert.IsInstanceOfType(ugcFileDetailsEnvelope, typeof(UgcFileDetailsEnvelope));
            }
        }
    }
}
