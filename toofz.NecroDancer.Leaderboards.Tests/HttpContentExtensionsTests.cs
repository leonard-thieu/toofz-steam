using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpContentExtensionsTests
    {
        [TestClass]
        public class CloneAsyncMethod
        {
            [TestMethod]
            public async Task ContentIsNull_ReturnsNull()
            {
                // Arrange
                HttpContent content = null;

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content);

                // Assert
                Assert.IsNull(clone);
            }

            [TestMethod]
            public async Task ClonesContent()
            {
                // Arrange
                var content = new StringContent("0123456789");

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content);

                // Assert
                var cloneContent = await clone.ReadAsStringAsync();
                Assert.AreEqual("0123456789", cloneContent);
            }

            [TestMethod]
            public async Task ClonesHeaders()
            {
                // Arrange
                var content = new StringContent("0123456789", Encoding.UTF8);

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content);

                // Assert
                Assert.AreEqual("utf-8", clone.Headers.ContentType.CharSet);
            }
        }
    }
}
