using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam
{
    class UgcHttpClientTests
    {
        public UgcHttpClientTests()
        {
            Client = new UgcHttpClient(Handler);
        }

        public MockHttpMessageHandler Handler { get; set; } = new MockHttpMessageHandler();
        public UgcHttpClient Client { get; set; }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();

                // Act
                var client = new UgcHttpClient(handler);

                // Assert
                Assert.IsInstanceOfType(client, typeof(UgcHttpClient));
            }
        }

        [TestClass]
        public class GetUgcFileAsync : UgcHttpClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                var requestUri = "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/";

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetUgcFileAsync(requestUri);
                });
            }

            [TestMethod]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string requestUri = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return Client.GetUgcFileAsync(requestUri);
                });
            }

            [TestMethod]
            public async Task ReturnsUgcFile()
            {
                // Arrange
                Handler
                    .When(HttpMethod.Get, "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/")
                    .Respond(new ByteArrayContent(Resources.UgcFile));
                var requestUri = "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/";

                // Act
                var ugcFile = await Client.GetUgcFileAsync(requestUri);

                // Assert
                Assert.IsInstanceOfType(ugcFile, typeof(byte[]));
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void DisposesHttpClient()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new UgcHttpClient(handler);

                // Act
                client.Dispose();

                // Assert
                Assert.AreEqual(1, handler.DisposeCount);
            }

            [TestMethod]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new UgcHttpClient(handler);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.AreEqual(1, handler.DisposeCount);
            }
        }
    }
}
