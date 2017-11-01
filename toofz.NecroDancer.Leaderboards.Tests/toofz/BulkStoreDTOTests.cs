using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class BulkStoreDTOTests
    {
        public class Serialization
        {
            [Fact]
            public void WithoutRowsAffected_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.BulkStoreDTOWithoutRowsAffected;

                // Act -> Assert
                Assert.Throws<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<BulkStoreDTO>(json);
                });
            }

            [Fact]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.BulkStoreDTO;

                // Act
                var bulkStore = JsonConvert.DeserializeObject<BulkStoreDTO>(json);

                // Assert
                Assert.IsAssignableFrom<BulkStoreDTO>(bulkStore);
                Assert.Equal(10, bulkStore.RowsAffected);
            }
        }
    }
}
