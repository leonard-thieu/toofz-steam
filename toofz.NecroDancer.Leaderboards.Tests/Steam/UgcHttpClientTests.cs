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
        [TestClass]
        public class GetUgcFileAsync
        {
            [TestMethod]
            public async Task RequestUriIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var ugcHttpClient = new UgcHttpClient(handler);
                string requestUri = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return ugcHttpClient.GetUgcFileAsync(requestUri);
                });
            }

            [TestMethod]
            public async Task ReturnsUgcFile()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(HttpMethod.Get, "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/")
                    .Respond(new ByteArrayContent(Resources.UgcFile));
                var ugcHttpClient = new UgcHttpClient(handler);
                var requestUri = "http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/";

                // Act
                var ugcFile = await ugcHttpClient.GetUgcFileAsync(requestUri);

                // Assert
                Assert.IsInstanceOfType(ugcFile, typeof(byte[]));
            }
        }
    }
}
