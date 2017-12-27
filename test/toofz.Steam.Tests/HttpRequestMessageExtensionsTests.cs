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
            [DisplayFact(nameof(ArgumentNullException))]
            public async Task RequestIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpRequestMessage request = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return request.CloneAsync();
                });
            }

            [DisplayFact(nameof(HttpRequestMessage.Version))]
            public async Task ClonesVersion()
            {
                // Arrange
                var request = new HttpRequestMessage();

                // Act
                var clone = await request.CloneAsync();

                // Assert
                Assert.Equal(new Version("1.1"), clone.Version);
            }

            [DisplayFact(nameof(HttpRequestMessage.Content))]
            public async Task ClonesContent()
            {
                // Arrange
                var request = new HttpRequestMessage();
                request.Content = new StringContent("0123456789");

                // Act
                var clone = await request.CloneAsync();

                // Assert
                var cloneContent = await clone.Content.ReadAsStringAsync();
                Assert.Equal("0123456789", cloneContent);
            }

            [DisplayFact(nameof(HttpRequestMessage.Method))]
            public async Task ClonesMethod()
            {
                // Arrange
                var request = new HttpRequestMessage { Method = HttpMethod.Post };

                // Act
                var clone = await request.CloneAsync();

                // Assert
                Assert.Equal(HttpMethod.Post, clone.Method);
            }

            [DisplayFact(nameof(HttpRequestMessage.RequestUri))]
            public async Task ClonesRequestUri()
            {
                // Arrange
                var request = new HttpRequestMessage { RequestUri = new Uri("http://example.org/") };

                // Act
                var clone = await request.CloneAsync();

                // Assert
                Assert.Equal(new Uri("http://example.org/"), clone.RequestUri);
            }

            [DisplayFact(nameof(HttpRequestMessage.Headers))]
            public async Task ClonesHeaders()
            {
                // Arrange
                var request = new HttpRequestMessage();
                request.Headers.Host = "example.org";

                // Act
                var clone = await request.CloneAsync();

                // Assert
                Assert.Equal("example.org", clone.Headers.Host);
            }

            [DisplayFact(nameof(HttpRequestMessage.Properties))]
            public async Task ClonesProperties()
            {
                // Arrange
                var request = new HttpRequestMessage();
                var property = new object();
                request.Properties.Add("myProp", property);

                // Act
                var clone = await request.CloneAsync();

                // Assert
                Assert.Equal(property, clone.Properties["myProp"]);
            }
        }
    }
}
