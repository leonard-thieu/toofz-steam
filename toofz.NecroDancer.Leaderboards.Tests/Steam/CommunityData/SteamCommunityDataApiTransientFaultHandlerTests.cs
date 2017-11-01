using toofz.NecroDancer.Leaderboards.Steam.CommunityData;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    public class SteamCommunityDataApiTransientFaultHandlerTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new SteamCommunityDataApiTransientFaultHandler();

                // Assert
                Assert.IsAssignableFrom<SteamCommunityDataApiTransientFaultHandler>(handler);
            }
        }
    }
}
