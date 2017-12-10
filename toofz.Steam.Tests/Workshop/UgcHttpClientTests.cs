using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using RichardSzalay.MockHttp;
using toofz.Steam.Workshop;
using toofz.Steam.Tests.Properties;
using Xunit;

namespace toofz.Steam.Tests.Workshop
{
    public class UgcHttpClientTests
    {
        public UgcHttpClientTests()
        {
            client = new UgcHttpClient(handler, telemetryClient);
        }

        private readonly MockHttpMessageHandler handler = new MockHttpMessageHandler();
        private readonly TelemetryClient telemetryClient = new TelemetryClient();
        private readonly UgcHttpClient client;

        public class IsTransientMethod
        {
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
                var isTransient = UgcHttpClient.IsTransient(ex);

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
                var isTransient = UgcHttpClient.IsTransient(ex);

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
                var isTransient = UgcHttpClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }
        }

        public class Constructor
        {
            [DisplayFact(nameof(UgcHttpClient))]
            public void ReturnsUgcHttpClient()
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
            [DisplayFact(nameof(ObjectDisposedException))]
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

            [DisplayFact("RequestUri", nameof(ArgumentNullException))]
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

            [DisplayFact]
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
            public DisposeMethod()
            {
                client = new UgcHttpClient(handler, true, telemetryClient);
            }

            private readonly SimpleHttpMessageHandler handler = new SimpleHttpMessageHandler();
            private readonly TelemetryClient telemetryClient = new TelemetryClient();
            private readonly UgcHttpClient client;

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
