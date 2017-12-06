using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace toofz.Steam.Tests
{
    public class HttpRequestMessageExtensionsTests
    {
        public class CloneAsyncMethod
        {
            [Fact]
            public async Task RequestIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpRequestMessage request = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return HttpRequestMessageExtensions.CloneAsync(request);
                });
            }

            [Fact]
            public async Task ClonesVersion()
            {
                // Arrange
                var request = new HttpRequestMessage();

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.Equal(new Version("1.1"), clone.Version);
            }

            [Fact]
            public async Task ClonesContent()
            {
                // Arrange
                var request = new HttpRequestMessage();
                request.Content = new StringContent("0123456789");

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                var cloneContent = await clone.Content.ReadAsStringAsync();
                Assert.Equal("0123456789", cloneContent);
            }

            [Fact]
            public async Task ClonesMethod()
            {
                // Arrange
                var request = new HttpRequestMessage { Method = HttpMethod.Post };

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.Equal(HttpMethod.Post, clone.Method);
            }

            [Fact]
            public async Task ClonesRequestUri()
            {
                // Arrange
                var request = new HttpRequestMessage { RequestUri = new Uri("http://example.org/") };

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.Equal(new Uri("http://example.org/"), clone.RequestUri);
            }

            [Fact]
            public async Task ClonesHeaders()
            {
                // Arrange
                var request = new HttpRequestMessage();
                request.Headers.Host = "example.org";

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.Equal("example.org", clone.Headers.Host);
            }

            [Fact]
            public async Task ClonesProperties()
            {
                // Arrange
                var request = new HttpRequestMessage();
                var property = new object();
                request.Properties.Add("myProp", property);

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.Equal(property, clone.Properties["myProp"]);
            }
        }
    }
}
