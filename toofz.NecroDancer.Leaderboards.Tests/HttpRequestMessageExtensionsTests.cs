using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpRequestMessageExtensionsTests
    {
        [TestClass]
        public class CloneAsyncMethod
        {
            [TestMethod]
            public async Task RequestIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpRequestMessage request = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return HttpRequestMessageExtensions.CloneAsync(request);
                });
            }

            [TestMethod]
            public async Task ClonesVersion()
            {
                // Arrange
                var request = new HttpRequestMessage();

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.AreEqual(new Version("1.1"), clone.Version);
            }

            [TestMethod]
            public async Task ClonesContent()
            {
                // Arrange
                var request = new HttpRequestMessage();
                request.Content = new StringContent("0123456789");

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                var cloneContent = await clone.Content.ReadAsStringAsync();
                Assert.AreEqual("0123456789", cloneContent);
            }

            [TestMethod]
            public async Task ClonesMethod()
            {
                // Arrange
                var request = new HttpRequestMessage { Method = HttpMethod.Post };

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.AreEqual(HttpMethod.Post, clone.Method);
            }

            [TestMethod]
            public async Task ClonesRequestUri()
            {
                // Arrange
                var request = new HttpRequestMessage { RequestUri = new Uri("http://example.org/") };

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.AreEqual(new Uri("http://example.org/"), clone.RequestUri);
            }

            [TestMethod]
            public async Task ClonesHeaders()
            {
                // Arrange
                var request = new HttpRequestMessage();
                request.Headers.Host = "example.org";

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.AreEqual("example.org", clone.Headers.Host);
            }

            [TestMethod]
            public async Task ClonesProperties()
            {
                // Arrange
                var request = new HttpRequestMessage();
                var property = new object();
                request.Properties.Add("myProp", property);

                // Act
                var clone = await HttpRequestMessageExtensions.CloneAsync(request);

                // Assert
                Assert.AreEqual(property, clone.Properties["myProp"]);
            }
        }
    }
}
