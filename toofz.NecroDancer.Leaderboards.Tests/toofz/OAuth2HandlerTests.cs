using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class OAuth2HandlerTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void UserNameIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = null;
                string password = "myPassword";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [TestMethod]
            public void UserNameIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "";
                string password = "myPassword";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [TestMethod]
            public void PasswordIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = "myUserName";
                string password = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [TestMethod]
            public void PasswordIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "myUserName";
                string password = "";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new OAuth2Handler(userName, password);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                string userName = "myUserName";
                string password = "myPassword";

                // Act
                var handler = new OAuth2Handler(userName, password);

                // Assert
                Assert.IsInstanceOfType(handler, typeof(OAuth2Handler));
            }
        }

        [TestClass]
        public class SendAsync
        {
            [TestMethod]
            public async Task Authorized_ReturnsResponse()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();
                mockHandler
                    .When("*")
                    .Respond(HttpStatusCode.OK);

                var bearerToken = new OAuth2AccessToken();
                var userName = "myUserName";
                var password = "myPassword";
                var handler = new TestingHttpMessageHandler(new OAuth2Handler(userName, password)
                {
                    InnerHandler = mockHandler,
                    BearerToken = bearerToken,
                });

                var mockRequest = new Mock<HttpRequestMessage>();

                // Act
                await handler.TestSendAsync(mockRequest.Object, CancellationToken.None);
                var authorization = mockRequest.Object.Headers.Authorization;

                // Assert
                Assert.AreEqual("Bearer", authorization.Scheme);
            }

            [TestMethod]
            public async Task Unauthorized_Authenticates()
            {
                // Arrange
                var mockHandler = new MockHttpMessageHandler();

                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer"));
                mockHandler
                    .Expect(Constants.FakeUri)
                    .Respond(req => response);

                mockHandler
                    .Expect(new Uri(Constants.FakeUri, "/token"))
                    .RespondJson(new OAuth2AccessToken
                    {
                        AccessToken = "fakeToken",
                        TokenType = "bearer",
                        UserName = "myUserName",
                    });

                mockHandler
                    .Expect(Constants.FakeUri)
                    .WithHeaders("Authorization", "Bearer fakeToken")
                    .Respond(HttpStatusCode.OK);

                var userName = "myUserName";
                var password = "myPassword";
                var handler = new TestingHttpMessageHandler(new OAuth2Handler(userName, password)
                {
                    InnerHandler = mockHandler,
                });

                var request = new HttpRequestMessage()
                {
                    RequestUri = Constants.FakeUri
                };

                // Act
                await handler.TestSendAsync(request, CancellationToken.None);
                var authorization = request.Headers.Authorization;

                // Assert
                mockHandler.VerifyNoOutstandingExpectation();
                Assert.AreEqual("Bearer", authorization.Scheme);
            }
        }
    }
}
