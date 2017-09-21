using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class BulkStoreDTOTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutRowsAffected_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.BulkStoreDTOWithoutRowsAffected;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<BulkStoreDTO>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.BulkStoreDTO;

                // Act
                var bulkStore = JsonConvert.DeserializeObject<BulkStoreDTO>(json);

                // Assert
                Assert.IsInstanceOfType(bulkStore, typeof(BulkStoreDTO));
                Assert.AreEqual(10, bulkStore.RowsAffected);
            }
        }
    }
}
