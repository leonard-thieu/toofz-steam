using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamRemoteStorage
{
    public class UgcFileDetailsEnvelopeTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutData_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.UgcFileDetailsEnvelopeWithoutData;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.UgcFileDetailsEnvelope;

                // Act
                var ugcFileDetailsEnvelope = JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);

                // Assert
                Assert.IsAssignableFrom<UgcFileDetailsEnvelope>(ugcFileDetailsEnvelope);
                Assert.IsAssignableFrom<UgcFileDetails>(ugcFileDetailsEnvelope.Data);
            }
        }
    }
}
