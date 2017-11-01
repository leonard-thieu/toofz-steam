using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    public class OAuth2HandlerTests
    {
        public class Constructor
        {
            [Fact]
            public void UserNameIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = null;
                string password = "myPassword";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [Fact]
            public void UserNameIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "";
                string password = "myPassword";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [Fact]
            public void PasswordIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = "myUserName";
                string password = null;

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [Fact]
            public void PasswordIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "myUserName";
                string password = "";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                string userName = "myUserName";
                string password = "myPassword";

                // Act
                var handler = new OAuth2Handler(userName, password);

                // Assert
                Assert.IsAssignableFrom<OAuth2Handler>(handler);
            }
        }


        public class SendAsync
        {
            [Fact]
            public async Task Authorized_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler.When("*").Respond(HttpStatusCode.OK);
                var bearerToken = new OAuth2BearerToken();
                var userName = "myUserName";
                var password = "myPassword";
                var handler = new HttpMessageHandlerAdapter(new OAuth2Handler(userName, password)
                {
                    InnerHandler = mockHandler,
                    BearerToken = bearerToken,
                });
                var mockRequest = new Mock<HttpRequestMessage>();
                var request = mockRequest.Object;
                var cancellationToken = CancellationToken.None;

                // Act
                await handler.PublicSendAsync(request, cancellationToken);

                // Assert
                var authorization = request.Headers.Authorization;
                Assert.Equal("Bearer", authorization.Scheme);
            }

            [Fact]
            public async Task Unauthorized_Authenticates()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer"));
                mockHandler.Expect("/").Respond(req => response);
                mockHandler.Expect("/token").Respond("application/json", Resources.OAuth2BearerToken);
                mockHandler.Expect("/").WithHeaders("Authorization", "Bearer myAccessToken").Respond(HttpStatusCode.OK);
                var userName = "myUserName";
                var password = "myPassword";
                var oAuth2Handler = new OAuth2Handler(userName, password) { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(oAuth2Handler);
                var request = new HttpRequestMessage { RequestUri = new Uri("http://example.org/") };
                var cancellationToken = CancellationToken.None;

                // Act
                await handler.PublicSendAsync(request, cancellationToken);

                // Assert
                mockHandler.VerifyNoOutstandingExpectation();
                Assert.Equal("myAccessToken", oAuth2Handler.BearerToken.AccessToken);
            }

            [Fact]
            public async Task UnauthorizedAndReceivedInvalidBearerToken_ThrowsInvalidDataException()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer"));
                mockHandler.Expect("/").Respond(req => response);
                mockHandler.Expect("/token").Respond("application/json", Resources.OAuth2BearerToken);
                var userName = "myUserNameThatWillNotMatch";
                var password = "myPassword";
                var oAuth2Handler = new OAuth2Handler(userName, password) { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(oAuth2Handler);
                var request = new HttpRequestMessage { RequestUri = new Uri("http://example.org/") };
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidDataException>(() =>
                {
                    return handler.PublicSendAsync(request, cancellationToken);
                });
            }

            [Fact]
            public async Task UnauthorizedAndReceivedNonBearerTokenResponse_ThrowsInvalidDataException()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer"));
                mockHandler.Expect("/").Respond(req => response);
                mockHandler.Expect("/token").Respond("application/json", Resources.HttpError);
                var userName = "myUserNameThatWillNotMatch";
                var password = "myPassword";
                var oAuth2Handler = new OAuth2Handler(userName, password) { InnerHandler = mockHandler };
                var handler = new HttpMessageHandlerAdapter(oAuth2Handler);
                var request = new HttpRequestMessage { RequestUri = new Uri("http://example.org/") };
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidDataException>(() =>
                {
                    return handler.PublicSendAsync(request, cancellationToken);
                });
            }
        }
    }
}
