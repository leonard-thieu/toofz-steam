using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Leaderboards.Steam.WebApi;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.WebApi
{
    class SteamWebApiTransientFaultHandlerTests
    {
        [TestClass]
        public class Costructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new SteamWebApiTransientFaultHandler();

                // Assert
                Assert.IsInstanceOfType(handler, typeof(SteamWebApiTransientFaultHandler));
            }
        }
    }
}
