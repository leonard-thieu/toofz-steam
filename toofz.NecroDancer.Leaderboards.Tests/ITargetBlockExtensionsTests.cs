using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ITargetBlockExtensionsTests
    {
        [TestClass]
        public class CheckSendAsync
        {
            [TestMethod]
            public async Task TargetIsNull_ThrowsArgumentNullException()
            {
                // Arrange -> Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return ITargetBlockExtensions.CheckSendAsync(null, (object)null);
                });
            }
        }
    }
}
