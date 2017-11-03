using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using Moq;
using RichardSzalay.MockHttp;
using toofz.TestsShared;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class LoggingHandlerTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var handler = new LoggingHandler();

                // Assert
                Assert.IsAssignableFrom<LoggingHandler>(handler);
            }
        }

        public class SendAsync
        {
            [Fact]
            public async Task LogsDebugStartDownload()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.OK);
                var loggingHandler = new LoggingHandler(log) { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(loggingHandler);

                // Act
                await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://fake.uri/"));

                // Assert
                mockLog.Verify(l => l.Debug("Start download http://fake.uri/"), Times.Once);
            }

            [Fact]
            public async Task LogsDebugEndDownload()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.OK);
                var loggingHandler = new LoggingHandler(log) { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(loggingHandler);

                // Act
                await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://fake.uri/"));

                // Assert
                mockLog.Verify(l => l.Debug("End download http://fake.uri/"), Times.Once);
            }

            [Fact]
            public async Task ReturnsResponse()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(new StringContent("myContent"));
                var loggingHandler = new LoggingHandler(log) { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(loggingHandler);

                // Act
                var response = await handler.PublicSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://fake.uri/"));
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal("myContent", content);
            }
        }
    }
}
