using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace toofz.Steam.Tests
{
    public class HttpContentExtensionsTests
    {
        public class CloneAsyncMethod
        {
            private HttpContent httpContent = new StringContent("", Encoding.UTF8);
            private Stream stream = Stream.Null;

            [DisplayFact(nameof(HttpContent))]
            public void HttpContentIsNull_ReturnsNull()
            {
                // Arrange
                httpContent = null;

                // Act
                var clone = httpContent.Clone(stream);

                // Assert
                Assert.Null(clone);
            }

            [DisplayFact(nameof(HttpContent))]
            public async Task ClonesHttpContent()
            {
                // Arrange
                stream = new MemoryStream(Encoding.UTF8.GetBytes("0123456789"));

                // Act
                var clone = httpContent.Clone(stream);

                // Assert
                var cloneContent = await clone.ReadAsStringAsync();
                Assert.Equal("0123456789", cloneContent);
            }

            [DisplayFact(nameof(HttpContent.Headers))]
            public void ClonesHeaders()
            {
                // Arrange -> Act
                var clone = httpContent.Clone(stream);

                // Assert
                Assert.Equal("utf-8", clone.Headers.ContentType.CharSet);
            }
        }

        public class ReadAsAsyncMethod
        {
            [DisplayFact(nameof(HttpContent), nameof(ArgumentNullException))]
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

            [DisplayFact]
            public async Task ResponseFailsDeserialization_ReturnsDefaultOfType()
            {
                // Arrange
                var httpContent = new StringContent("");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.Null(content);
            }

            [DisplayFact]
            public async Task ReturnsDeserializedObject()
            {
                // Arrange
                var httpContent = new StringContent("{\"MyProperty\":\"MyValue\"}");

                // Act
                var content = await httpContent.ReadAsAsync<TestDto>();

                // Assert
                Assert.Equal("MyValue", content.MyProperty);
            }

            [DisplayFact]
            public async Task DeserializationError_ThrowsJsonSerializationException()
            {
                // Arrange
                var httpContent = new StringContent("{\"NotMyProperty\":\"MyValue\"}");

                // Act -> Assert
                await Assert.ThrowsAsync<JsonSerializationException>(() =>
                {
                    return httpContent.ReadAsAsync<TestDto>();
                });
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
