using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamRemoteStorage
{
    class UgcFileDetailsTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutFileName_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.UgcFileDetailsWithoutFileName;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);
                });
            }

            [TestMethod]
            public void WithoutUrl_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.UgcFileDetailsWithoutUrl;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);
                });
            }

            [TestMethod]
            public void WithoutSize_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.UgcFileDetailsWithoutSize;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.UgcFileDetails;

                // Act
                var ugcFileDetails = JsonConvert.DeserializeObject<UgcFileDetails>(json);

                // Assert
                Assert.IsInstanceOfType(ugcFileDetails, typeof(UgcFileDetails));
                Assert.AreEqual("2/9/2014_score191_zone1_level2", ugcFileDetails.FileName);
                Assert.AreEqual("http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/", ugcFileDetails.Url);
                Assert.AreEqual(1558, ugcFileDetails.Size);
            }
        }
    }
}
