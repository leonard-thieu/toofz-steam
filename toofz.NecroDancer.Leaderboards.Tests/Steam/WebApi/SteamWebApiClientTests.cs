using System;
using System.Net;
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

        private readonly MockHttpMessageHandler handler = new MockHttpMessageHandler();
        private readonly TelemetryClient telemetryClient = new TelemetryClient();
        private readonly SteamWebApiClient client;

        public class IsTransientMethod
        {
            [Theory]
            [InlineData(408)]
            [InlineData(429)]
            [InlineData(500)]
            [InlineData(502)]
            [InlineData(503)]
            [InlineData(504)]
            public void HttpRequestStatusExceptionAndStatusCodeIsTransient_HandlesException(HttpStatusCode statusCode)
            {
                // Arrange
                var ex = new HttpRequestStatusException(statusCode, new Uri("http://example.org"));

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            public void ExIsHttpRequestStatusExceptionAndStatusCodeIsNotTransient_ReturnsFalse()
            {
                // Arrange
                var statusCode = HttpStatusCode.Forbidden;
                var ex = new HttpRequestStatusException(statusCode, new Uri("http://example.org"));

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }

            [Theory]
            [InlineData(WebExceptionStatus.ConnectFailure)]
            [InlineData(WebExceptionStatus.SendFailure)]
            [InlineData(WebExceptionStatus.PipelineFailure)]
            [InlineData(WebExceptionStatus.RequestCanceled)]
            [InlineData(WebExceptionStatus.ConnectionClosed)]
            [InlineData(WebExceptionStatus.KeepAliveFailure)]
            [InlineData(WebExceptionStatus.UnknownError)]
            public void ExIsHttpRequestExceptionAndInnerExceptionIsWebExceptionAndStatusIsTransient_ReturnsTrue(WebExceptionStatus status)
            {
                // Arrange
                var inner = new WebException(null, status);
                var ex = new HttpRequestException(null, inner);

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [Fact]
            public void ExIsHttpRequestExceptionAndInnerExceptionIsWebExceptionAndStatusIsNotTransient_ReturnsFalse()
            {
                // Arrange
                var status = WebExceptionStatus.NameResolutionFailure;
                var inner = new WebException(null, status);
                var ex = new HttpRequestException(null, inner);

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }

            [Fact]
            public void ExIsHttpRequestExceptionAndInnerExceptionIsNotWebException_ReturnsFalse()
            {
                // Arrange
                var inner = new Exception();
                var ex = new HttpRequestException(null, inner);

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }

            [Fact]
            public void ReturnsFalse()
            {
                // Arrange
                var ex = new Exception();

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }
        }

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
            public GetPlayerSummariesAsync()
            {
                client.SteamWebApiKey = "mySteamWebApiKey";
            }

            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                client.Dispose();
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
            public GetUgcFileDetailsAsync()
            {
                client.SteamWebApiKey = "mySteamWebApiKey";
            }

            private uint appId = 247080;
            private long ugcId = 22837952671856412;

            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                client.Dispose();

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
            public DisposeMethod()
            {
                client = new SteamWebApiClient(handler, true, telemetryClient);
            }

            private readonly SimpleHttpMessageHandler handler = new SimpleHttpMessageHandler();
            private readonly TelemetryClient telemetryClient = new TelemetryClient();
            private readonly SteamWebApiClient client;

            [Fact]
            public void DisposesHttpClient()
            {
                // Arrange -> Act
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }

            [Fact]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange -> Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }
        }
    }
}
