using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class ProgressReporterHttpClientTests
    {
        public class GetAsyncMethod_String
        {
            [Fact]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new ProgressReporterHttpClient(handler);
                string requestUri = null;
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return httpClient.GetAsync(requestUri, progress, cancellationToken);
                });
            }

            [Fact]
            public async Task ReportsContentLength()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
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
                var httpClient = new ProgressReporterHttpClient(handler);
                var requestUri = "http://example.org/";
                var mockProgress = new Mock<IProgress<long>>();
                var progress = mockProgress.Object;
                var cancellationToken = CancellationToken.None;

                // Act
                await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                mockProgress.Verify(p => p.Report(24), Times.Once);
            }

            [Fact]
            public async Task ReturnsResponse()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new ProgressReporterHttpClient(handler);
                var requestUri = "http://example.org/";
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act
                var response = await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }
        }


        public class GetAsync_Uri
        {
            [Fact]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new ProgressReporterHttpClient(handler);
                Uri requestUri = null;
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return httpClient.GetAsync(requestUri, progress, cancellationToken);
                });
            }

            [Fact]
            public async Task ReportsContentLength()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
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
                var httpClient = new ProgressReporterHttpClient(handler);
                var requestUri = new Uri("http://example.org/");
                var mockProgress = new Mock<IProgress<long>>();
                var progress = mockProgress.Object;
                var cancellationToken = CancellationToken.None;

                // Act
                await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                mockProgress.Verify(p => p.Report(24), Times.Once);
            }

            [Fact]
            public async Task ReturnsResponse()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new ProgressReporterHttpClient(handler);
                var requestUri = new Uri("http://example.org/");
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act
                var response = await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                Assert.IsAssignableFrom<HttpResponseMessage>(response);
            }
        }
    }
}
