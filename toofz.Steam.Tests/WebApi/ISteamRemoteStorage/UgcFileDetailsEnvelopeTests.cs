using Newtonsoft.Json;
using toofz.Steam.WebApi.ISteamRemoteStorage;
using toofz.Steam.Tests.Properties;
using Xunit;

namespace toofz.Steam.Tests.WebApi.ISteamRemoteStorage
{
    public class UgcFileDetailsEnvelopeTests
    {
        public class Serialization
        {
            [DisplayFact(nameof(UgcFileDetailsEnvelope.Data))]
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

            [DisplayFact]
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
