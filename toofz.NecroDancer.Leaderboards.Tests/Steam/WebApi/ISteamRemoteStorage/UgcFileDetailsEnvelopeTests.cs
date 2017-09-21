using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamRemoteStorage
{
    class UgcFileDetailsEnvelopeTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutData_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.UgcFileDetailsEnvelopeWithoutData;

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
                var json = Resources.UgcFileDetailsEnvelope;

                // Act
                var ugcFileDetailsEnvelope = JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);

                // Assert
                Assert.IsInstanceOfType(ugcFileDetailsEnvelope, typeof(UgcFileDetailsEnvelope));
                Assert.IsInstanceOfType(ugcFileDetailsEnvelope.Data, typeof(UgcFileDetails));
            }
        }
    }
}
