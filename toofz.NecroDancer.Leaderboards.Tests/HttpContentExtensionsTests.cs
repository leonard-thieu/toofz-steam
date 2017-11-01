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
                var clone = await HttpContentExtensions.CloneAsync(httpContent);

                // Assert
                Assert.Null(clone);
            }

            [Fact]
            public async Task ClonesContent()
            {
                // Arrange
                var content = new StringContent("0123456789");

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content);

                // Assert
                var cloneContent = await clone.ReadAsStringAsync();
                Assert.Equal("0123456789", cloneContent);
            }

            [Fact]
            public async Task ClonesHeaders()
            {
                // Arrange
                var content = new StringContent("0123456789", Encoding.UTF8);

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content);

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
                var clone = await HttpContentExtensions.CloneAsync(httpContent, stream, cancellationToken);

                // Assert
                Assert.Null(clone);
            }

            [Fact]
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
                Assert.Equal("0123456789", cloneContent);
            }

            [Fact]
            public async Task ClonesHeaders()
            {
                // Arrange
                var content = new StringContent("0123456789", Encoding.UTF8);
                var stream = Stream.Null;
                var cancellationToken = CancellationToken.None;

                // Act
                var clone = await HttpContentExtensions.CloneAsync(content, stream, cancellationToken);

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
                    return HttpContentExtensions.ReadAsAsync<object>(httpContent);
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
                Assert.IsAssignableFrom<TestDto>(content);
                Assert.Equal("MyValue", content.MyProperty);
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
