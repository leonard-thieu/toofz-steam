using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class HttpContentExtensionsTests
    {
        public class CloneAsyncMethod
        {
            [Fact]
            public async Task HttpContentIsNull_ReturnsNull()
            {
                // Arrange
                HttpContent httpContent = null;

                // Act
                var clone = await httpContent.CloneAsync();

                // Assert
                Assert.Null(clone);
            }

            [Fact]
            public async Task ClonesContent()
            {
                // Arrange
                var httpContent = new StringContent("0123456789");

                // Act
                var clone = await httpContent.CloneAsync();

                // Assert
                var cloneContent = await clone.ReadAsStringAsync();
                Assert.Equal("0123456789", cloneContent);
            }

            [Fact]
            public async Task ClonesHeaders()
            {
                // Arrange
                var httpContent = new StringContent("0123456789", Encoding.UTF8);

                // Act
                var clone = await httpContent.CloneAsync();

                // Assert
                Assert.Equal("utf-8", clone.Headers.ContentType.CharSet);
            }
        }

        public class CloneAsyncMethod_Stream
        {
            [Fact]
            public async Task HttpContentIsNull_ReturnsNull()
            {
                // Arrange
                HttpContent httpContent = null;
                var stream = Stream.Null;
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await httpContent.CloneAsync(stream, cancellationToken);

                // Assert
                Assert.Null(clone);
            }

            [Fact]
            public async Task ClonesContent()
            {
                // Arrange
                var httpContent = new StringContent("");
                var stream = new MemoryStream(Encoding.UTF8.GetBytes("0123456789"));
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await httpContent.CloneAsync(stream, cancellationToken);

                // Assert
                var cloneContent = await clone.ReadAsStringAsync();
                Assert.Equal("0123456789", cloneContent);
            }

            [Fact]
            public async Task ClonesHeaders()
            {
                // Arrange
                var httpContent = new StringContent("0123456789", Encoding.UTF8);
                var stream = Stream.Null;
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await httpContent.CloneAsync(stream, cancellationToken);

                // Assert
                Assert.Equal("utf-8", clone.Headers.ContentType.CharSet);
            }
        }

        public class ReadAsAsyncMethod
        {
            [Fact]
            public async Task HttpContentIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpContent httpContent = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return httpContent.ReadAsAsync<object>();
                });
            }

            [Fact]
            public async Task ResponseFailsDeserialization_ReturnsDefaultOfType()
            {
                // Arrange
                var httpContent = new StringContent("");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.Null(content);
            }

            [Fact]
            public async Task ReturnsDeserializedObject()
            {
                // Arrange
                var httpContent = new StringContent("{\"MyProperty\":\"MyValue\"}");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.Equal("MyValue", content.MyProperty);
            }

            [Fact]
            public async Task DeserializationError_ReturnsDefault()
            {
                // Arrange
                var httpContent = new StringContent("{\"NotMyProperty\":\"MyValue\"}");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.Null(content);
            }

            [DataContract]
            private class TestDto
            {
                [DataMember(IsRequired = true)]
                public string MyProperty { get; set; }
            }
        }
    }
}
