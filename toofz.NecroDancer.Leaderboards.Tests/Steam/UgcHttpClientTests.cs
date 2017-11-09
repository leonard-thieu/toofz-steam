using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam
{
    public class UgcHttpClientTests
    {
        public UgcHttpClientTests()
        {
            client = new UgcHttpClient(handler, telemetryClient);
        }

        private MockHttpMessageHandler handler = new MockHttpMessageHandler();
        private TelemetryClient telemetryClient = new TelemetryClient();
        public UgcHttpClient client { get; set; }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var telemetryClient = new TelemetryClient();

                // Act
                var client = new UgcHttpClient(handler, telemetryClient);

                // Assert
                Assert.IsAssignableFrom<UgcHttpClient>(client);
            }
        }

        public class GetUgcFileAsync : UgcHttpClientTests
        {
            [Fact]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                client.Dispose();
                var requestUri = "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/";

                // Act -> Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                {
                    return client.GetUgcFileAsync(requestUri);
                });
            }

            [Fact]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string requestUri = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return client.GetUgcFileAsync(requestUri);
                });
            }

            [Fact]
            public async Task ReturnsUgcFile()
            {
                // Arrange
                handler
                    .When(HttpMethod.Get, "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/")
                    .Respond(new StringContent(Resources.UgcFile));
                var requestUri = "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/";

                // Act
                var ugcFile = await client.GetUgcFileAsync(requestUri);

                // Assert
                Assert.IsAssignableFrom<byte[]>(ugcFile);
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
                var client = new UgcHttpClient(handler, telemetryClient);

                // Act
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }

            [Fact]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange
                var client = new UgcHttpClient(handler, telemetryClient);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.Equal(1, handler.DisposeCount);
            }
        }
    }
}
