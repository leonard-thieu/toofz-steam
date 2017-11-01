using System.Linq;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class ReplaysEnvelopeTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutTotal_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplaysEnvelopeWithoutTotal;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplaysEnvelope>(json);
                });
            }

            [Fact]
            public void WithoutReplays_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplaysEnvelopeWithoutReplays;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplaysEnvelope>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.ReplaysEnvelope;

                // Act
                var replaysEnvelope = JsonConvert.DeserializeObject<ReplaysEnvelope>(json);

                // Assert
                Assert.IsAssignableFrom<ReplaysEnvelope>(replaysEnvelope);
                Assert.Equal(43767, replaysEnvelope.Total);
                var replays = replaysEnvelope.Replays.ToList();
                Assert.Equal(20, replays.Count);
                foreach (var r in replays)
                {
                    Assert.IsAssignableFrom<ReplayDTO>(r);
                }
            }
        }
    }
}
