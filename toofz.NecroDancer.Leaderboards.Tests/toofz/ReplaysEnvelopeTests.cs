using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class ReplaysEnvelopeTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutTotal_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplaysEnvelopeWithoutTotal;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplaysEnvelope>(json);
                });
            }

            [TestMethod]
            public void WithoutReplays_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.ReplaysEnvelopeWithoutReplays;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<ReplaysEnvelope>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.ReplaysEnvelope;

                // Act
                var replaysEnvelope = JsonConvert.DeserializeObject<ReplaysEnvelope>(json);

                // Assert
                Assert.IsInstanceOfType(replaysEnvelope, typeof(ReplaysEnvelope));
                Assert.AreEqual(43767, replaysEnvelope.Total);
                var replays = replaysEnvelope.Replays.ToList();
                Assert.AreEqual(20, replays.Count);
                CollectionAssert.AllItemsAreInstancesOfType(replays, typeof(ReplayDTO));
            }
        }
    }
}
