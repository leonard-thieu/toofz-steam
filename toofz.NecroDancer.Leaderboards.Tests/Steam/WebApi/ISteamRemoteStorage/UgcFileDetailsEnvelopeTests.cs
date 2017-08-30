using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Steam.WebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi.ISteamRemoteStorage
{
    class UgcFileDetailsEnvelopeTests
    {
        [TestClass]
        public class Deserialization
        {
            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.UgcFileDetails;

                // Act
                var ugcFileDetails = JsonConvert.DeserializeObject<UgcFileDetailsEnvelope>(json);

                // Assert
                Assert.IsInstanceOfType(ugcFileDetails, typeof(UgcFileDetailsEnvelope));
                Assert.IsNotNull(ugcFileDetails.Data);
            }
        }
    }
}
