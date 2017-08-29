using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApiContrib.Content;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpContentExtensionsTests
    {
        [TestClass]
        public class ProcessContentAsyncTests
        {
            [TestMethod]
            public async Task ContentIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpContent content = null;
                IProgress<long> progress = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return HttpContentExtensions.ProcessContentAsync(content, progress);
                });
            }

            [TestMethod]
            public async Task ReturnsSeekableStream()
            {
                // Arrange
                HttpContent content = new StringContent("myContent");
                IProgress<long> progress = null;

                // Act
                using (var copied = await HttpContentExtensions.ProcessContentAsync(content, progress))
                {
                    // Assert
                    Assert.IsTrue(copied.CanSeek);
                }
            }

            [TestMethod]
            public async Task ContentIsGzippedAndProgressIsNotNull_ReportsCompressedSize()
            {
                // Arrange
                HttpContent content = new CompressedContent(new ByteArrayContent(new byte[1024]), "gzip");
                var mockProgress = new Mock<IProgress<long>>();
                IProgress<long> progress = mockProgress.Object;

                // Act
                using (var copied = await HttpContentExtensions.ProcessContentAsync(content, progress))
                {
                    // Assert
                    mockProgress.Verify(p => p.Report(29));
                }
            }

            [TestMethod]
            public async Task ProgressIsNotNull_ReportsSize()
            {
                // Arrange
                HttpContent content = new ByteArrayContent(new byte[1024]);
                var mockProgress = new Mock<IProgress<long>>();
                IProgress<long> progress = mockProgress.Object;

                // Act
                using (var copied = await HttpContentExtensions.ProcessContentAsync(content, progress))
                {
                    // Assert
                    mockProgress.Verify(p => p.Report(1024));
                }
            }

            [TestMethod]
            public async Task ContentIsGzipped_ReturnsDecompressedStream()
            {
                // Arrange
                HttpContent content = new CompressedContent(new StringContent("myContent"), "gzip");
                IProgress<long> progress = null;

                // Act
                using (var copied = await HttpContentExtensions.ProcessContentAsync(content, progress))
                using (var sr = new StreamReader(copied))
                {
                    // Assert
                    Assert.AreEqual("myContent", sr.ReadToEnd());
                }
            }

            [TestMethod]
            public async Task ContentIsGzipped_ReturnsSeekableStream()
            {
                // Arrange
                HttpContent content = new CompressedContent(new StringContent("myContent"), "gzip");
                IProgress<long> progress = null;

                // Act
                using (var copied = await HttpContentExtensions.ProcessContentAsync(content, progress))
                {
                    // Assert
                    Assert.IsTrue(copied.CanSeek);
                }
            }
        }
    }
}
