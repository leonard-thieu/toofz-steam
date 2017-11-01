using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class ReplayDTOTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutId_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutId;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [Fact]
            public void WithoutError_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutError;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [Fact]
            public void WithoutSeed_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutSeed;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [Fact]
            public void WithoutVersion_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutVersion;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [Fact]
            public void WithoutKilledBy_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutKilledBy;

                // Act
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.ReplayDTO;

                // Act
                var replay = JsonConvert.DeserializeObject<ReplayDTO>(json);

                // Assert
                Assert.IsAssignableFrom<ReplayDTO>(replay);
                Assert.Equal(844845073340377377, replay.Id);
                Assert.Null(replay.Error);
                Assert.Null(replay.Seed);
                Assert.Null(replay.Version);
                Assert.Null(replay.KilledBy);
            }
        }
    }
}
