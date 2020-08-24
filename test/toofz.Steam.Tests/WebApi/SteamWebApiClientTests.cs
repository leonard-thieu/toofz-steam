using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using RichardSzalay.MockHttp;
using toofz.Steam.WebApi;
using toofz.Steam.WebApi.ISteamRemoteStorage;
using toofz.Steam.WebApi.ISteamUser;
using toofz.Steam.Tests.Properties;
using Xunit;

namespace toofz.Steam.Tests.WebApi
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
            [DisplayTheory(nameof(HttpRequestStatusException), nameof(HttpRequestStatusException.StatusCode))]
            [InlineData(408)]
            [InlineData(429)]
            [InlineData(500)]
            [InlineData(502)]
            [InlineData(503)]
            [InlineData(504)]
            public void HttpRequestStatusExceptionAndStatusCodeIsTransient_ReturnsTrue(HttpStatusCode statusCode)
            {
                // Arrange
                var ex = new HttpRequestStatusException(statusCode, new Uri("http://example.org"));

                // Act
                var isTransient = SteamWebApiClient.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [DisplayFact(nameof(HttpRequestStatusException), nameof(HttpRequestStatusException.StatusCode))]
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

            [DisplayTheory(nameof(HttpRequestException), nameof(HttpRequestException.InnerException), nameof(WebException), nameof(WebException.Status))]
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

            [DisplayFact(nameof(HttpRequestException), nameof(HttpRequestException.InnerException), nameof(WebException), nameof(WebException.Status))]
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

            [DisplayFact(nameof(HttpRequestException), nameof(HttpRequestException.InnerException), nameof(WebException))]
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

            [DisplayFact]
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
            [DisplayFact(nameof(SteamWebApiClient))]
            public void ReturnsSteamWebApiClient()
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

            [DisplayFact(nameof(ObjectDisposedException))]
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

            [DisplayFact(nameof(SteamWebApiClient.SteamWebApiKey), nameof(InvalidOperationException))]
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

            [DisplayFact("SteamIds", nameof(ArgumentNullException))]
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

            [DisplayFact("SteamIds", nameof(ArgumentException))]
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

            [DisplayFact]
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

            [Fact(DisplayName = "Response contains â, Does not throw " + nameof(DecoderFallbackException))]
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

            [Fact]
            public async Task Something()
            {
                // Arrange
                var steamIds = new long[] { 76561197960435530 };
                handler
                    .When(HttpMethod.Get, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002")
                    .WithQueryString("key", client.SteamWebApiKey)
                    .WithQueryString("steamids", string.Join(",", steamIds))
                    .Respond(new StringContent(Resources.PlayerSummariesWithoutLastLogOff, Encoding.Default, "application/json"));

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

            [DisplayFact(nameof(ObjectDisposedException))]
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

            [DisplayFact(nameof(SteamWebApiClient.SteamWebApiKey), nameof(InvalidOperationException))]
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

            [DisplayFact]
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

            [DisplayFact(nameof(HttpClient))]
            public void DisposesHttpClient()
            {
                // Arrange -> Act
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }

            [DisplayFact(nameof(HttpClient))]
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
