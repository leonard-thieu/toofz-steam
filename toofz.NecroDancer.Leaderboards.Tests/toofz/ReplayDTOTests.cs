using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class ReplayDTOTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutId_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutId;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutError_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutError;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutSeed_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutSeed;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutVersion_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutVersion;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [TestMethod]
            public void WithoutKilledBy_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplayDTOWithoutKilledBy;

                // Act
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplayDTO>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.ReplayDTO;

                // Act
                var replay = JsonConvert.DeserializeObject<ReplayDTO>(json);

                // Assert
                Assert.IsInstanceOfType(replay, typeof(ReplayDTO));
                Assert.AreEqual(844845073340377377, replay.Id);
                Assert.IsNull(replay.Error);
                Assert.IsNull(replay.Seed);
                Assert.IsNull(replay.Version);
                Assert.IsNull(replay.KilledBy);
            }
        }
    }
}
