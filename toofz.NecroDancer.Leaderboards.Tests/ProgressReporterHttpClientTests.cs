using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ProgressReporterHttpClientTests
    {
        [TestClass]
        public class GetAsyncMethod_String
        {
            [TestMethod]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new EmptyProgressReporterHttpClient(handler);
                string requestUri = null;
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return httpClient.GetAsync(requestUri, progress, cancellationToken);
                });
            }

            [TestMethod]
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
                var httpClient = new EmptyProgressReporterHttpClient(handler);
                var requestUri = "http://example.org/";
                var mockProgress = new Mock<IProgress<long>>();
                var progress = mockProgress.Object;
                var cancellationToken = CancellationToken.None;

                // Act
                await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                mockProgress.Verify(p => p.Report(24), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsResponse()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new EmptyProgressReporterHttpClient(handler);
                var requestUri = "http://example.org/";
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act
                var response = await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }
        }

        [TestClass]
        public class GetAsync_Uri
        {
            [TestMethod]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new EmptyProgressReporterHttpClient(handler);
                Uri requestUri = null;
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return httpClient.GetAsync(requestUri, progress, cancellationToken);
                });
            }

            [TestMethod]
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
                var httpClient = new EmptyProgressReporterHttpClient(handler);
                var requestUri = new Uri("http://example.org/");
                var mockProgress = new Mock<IProgress<long>>();
                var progress = mockProgress.Object;
                var cancellationToken = CancellationToken.None;

                // Act
                await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                mockProgress.Verify(p => p.Report(24), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsResponse()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var httpClient = new EmptyProgressReporterHttpClient(handler);
                var requestUri = new Uri("http://example.org/");
                IProgress<long> progress = null;
                var cancellationToken = CancellationToken.None;

                // Act
                var response = await httpClient.GetAsync(requestUri, progress, cancellationToken);

                // Assert
                Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            }
        }

        class EmptyProgressReporterHttpClient : ProgressReporterHttpClient
        {
            public EmptyProgressReporterHttpClient(HttpMessageHandler handler) : base(handler) { }
        }
    }
}
