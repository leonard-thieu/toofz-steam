using toofz.NecroDancer.Leaderboards.Steam.CommunityData;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    public class SteamCommunityDataTransientFaultHandlerTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new SteamCommunityDataTransientFaultHandler();

                // Assert
                Assert.IsAssignableFrom<SteamCommunityDataTransientFaultHandler>(handler);
            }
        }
    }
}
