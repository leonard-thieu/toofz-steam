using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Leaderboards.Steam.CommunityData;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    class SteamCommunityDataApiTransientFaultHandlerTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new SteamCommunityDataApiTransientFaultHandler();

                // Assert
                Assert.IsInstanceOfType(handler, typeof(SteamCommunityDataApiTransientFaultHandler));
            }
        }
    }
}
