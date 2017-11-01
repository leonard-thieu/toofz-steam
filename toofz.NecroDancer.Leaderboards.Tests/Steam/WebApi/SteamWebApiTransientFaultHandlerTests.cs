using toofz.NecroDancer.Leaderboards.Steam.WebApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    public class SteamWebApiTransientFaultHandlerTests
    {
        public class Costructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new SteamWebApiTransientFaultHandler();

                // Assert
                Assert.IsAssignableFrom<SteamWebApiTransientFaultHandler>(handler);
            }
        }
    }
}
