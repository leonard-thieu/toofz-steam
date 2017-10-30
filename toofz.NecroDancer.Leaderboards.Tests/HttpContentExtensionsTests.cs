using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal class HttpContentExtensionsTests
    {
        [TestClass]
        public class CloneAsyncMethod
        {
            [TestMethod]
            public async Task HttpContentIsNull_ReturnsNull()
            {
                // Arrange
                HttpContent httpContent = null;

                // Act
                var clone = await HttpContentExtensions.CloneAsync(httpContent);

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

        [TestClass]
        public class CloneAsyncMethod_Stream
        {
            [TestMethod]
            public async Task HttpContentIsNull_ReturnsNull()
            {
                // Arrange
                HttpContent httpContent = null;
                var stream = Stream.Null;
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await HttpContentExtensions.CloneAsync(httpContent, stream, cancellationToken);

                // Assert
                Assert.IsNull(clone);
            }

            [TestMethod]
            public async Task ClonesContent()
            {
                // Arrange
                var content = new StringContent("");
                var stream = new MemoryStream(Encoding.UTF8.GetBytes("0123456789"));
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content, stream, cancellationToken);

                // Assert
                var cloneContent = await clone.ReadAsStringAsync();
                Assert.AreEqual("0123456789", cloneContent);
            }

            [TestMethod]
            public async Task ClonesHeaders()
            {
                // Arrange
                var content = new StringContent("0123456789", Encoding.UTF8);
                var stream = Stream.Null;
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content, stream, cancellationToken);

                // Assert
                Assert.AreEqual("utf-8", clone.Headers.ContentType.CharSet);
            }
        }

        [TestClass]
        public class ReadAsAsyncMethod
        {
            [TestMethod]
            public async Task HttpContentIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpContent httpContent = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return HttpContentExtensions.ReadAsAsync<object>(httpContent);
                });
            }

            [TestMethod]
            public async Task ResponseFailsDeserialization_ReturnsDefaultOfType()
            {
                // Arrange
                var httpContent = new StringContent("");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.IsNull(content);
            }

            [TestMethod]
            public async Task ReturnsDeserializedObject()
            {
                // Arrange
                var httpContent = new StringContent("{\"MyProperty\":\"MyValue\"}");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.IsInstanceOfType(content, typeof(TestDto));
                Assert.AreEqual("MyValue", content.MyProperty);
            }

            [DataContract]
            class TestDto
            {
                [DataMember(IsRequired = true)]
                public string MyProperty { get; set; }
            }
        }
    }
}
