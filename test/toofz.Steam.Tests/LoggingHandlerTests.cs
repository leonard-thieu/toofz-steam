using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace toofz.Steam.Tests
{
    public class LoggingHandlerTests
    {
        public LoggingHandlerTests()
        {
            var loggingHandler = new LoggingHandler(mockLog.Object) { InnerHandler = mockHandler };
            handler = new HttpMessageHandlerAdapter(loggingHandler);
        }

        private readonly MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
        private readonly Mock<ILog> mockLog = new Mock<ILog>();
        private readonly HttpMessageHandlerAdapter handler;

        public class Constructor
        {
            [DisplayFact(nameof(LoggingHandler))]
            public void ReturnsLoggingHandler()
            {
                // Arrange -> Act
                var handler = new LoggingHandler();

                // Assert
                Assert.IsAssignableFrom<LoggingHandler>(handler);
            }
        }

        public class SendAsync : LoggingHandlerTests
        {
            [DisplayFact(nameof(ArgumentNullException))]
            public async Task RequsetIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpRequestMessage request = null;

                // Act -> Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                {
                    return handler.PublicSendAsync(request);
                });
            }

            [DisplayFact]
            public async Task LogsDebugStartDownload()
            {
                // Arrange
                mockLog.Setup(l => l.IsDebugEnabled).Returns(true);
                mockHandler.When("*").Respond(HttpStatusCode.OK);
                var request = new HttpRequestMessage(HttpMethod.Get, "http://fake.uri/");

                // Act
                await handler.PublicSendAsync(request);

                // Assert
                mockLog.Verify(l => l.Debug("Start download http://fake.uri/"), Times.Once);
            }

            [DisplayFact]
            public async Task LogsDebugEndDownload()
            {
                // Arrange
                mockLog.Setup(l => l.IsDebugEnabled).Returns(true);
                mockHandler.When("*").Respond(HttpStatusCode.OK);
                var request = new HttpRequestMessage(HttpMethod.Get, "http://fake.uri/");

                // Act
                await handler.PublicSendAsync(request);

                // Assert
                mockLog.Verify(l => l.Debug("End download http://fake.uri/"), Times.Once);
            }

            [DisplayFact]
            public async Task ReturnsResponse()
            {
                // Arrange
                mockHandler.When("*").Respond(HttpStatusCode.OK);
                var request = new HttpRequestMessage(HttpMethod.Get, "http://fake.uri/");

                // Act
                var response = await handler.PublicSendAsync(request);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
