using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ProgressReporterHttpClientTests
    {
        public ProgressReporterHttpClientTests()
        {
            httpClient = new ProgressReporterHttpClient(handler, true, telemetryClient);
        }

        private MockHttpMessageHandler handler = new MockHttpMessageHandler();
        private TelemetryClient telemetryClient = new TelemetryClient();
        private ProgressReporterHttpClient httpClient;

        public class GetAsyncMethod_String : ProgressReporterHttpClientTests
        {
            [Fact]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var operationName = "myOperationName";
                string requestUri = null;
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return httpClient.GetAsync(operationName, requestUri, progress, cancellationToken);
                });
            }

            [Fact]
            public async Task ReportsContentLength()
            {
                // Arrange
                handler
                    .When("*")
                    .Respond(() =>
                    {
                        var response = new HttpResponseMessage();
                        var content = new StringContent("");
                        content.Headers.ContentLength = 24;
                        response.Content = content;

                        return Task.FromResult(response);
                    });
                var operationName = "myOperationName";
                var requestUri = "http://example.org/";
                var mockProgress = new Mock<IProgress<long>>();
                var progress = mockProgress.Object;
                var cancellationToken = CancellationToken.None;

                // Act
                await httpClient.GetAsync(operationName, requestUri, progress, cancellationToken);

                // Assert
                mockProgress.Verify(p => p.Report(24), Times.Once);
            }

            [Fact]
            public async Task ReturnsResponse()
            {
                // Arrange
                var operationName = "myOperationName";
                var requestUri = "http://example.org/";
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act
                var response = await httpClient.GetAsync(operationName, requestUri, progress, cancellationToken);

                // Assert
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }
        }
    }
}
