using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class HttpClientExtensionsTests
    {
        [TestClass]
        public class PostAsJsonAsyncMethod
        {
            [TestMethod]
            public async Task HttpClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                HttpClient httpClient = null;
                var requestUri = "http://localhost/";
                var value = new object();
                var cancellationToken = CancellationToken.None;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return HttpClientExtensions.PostAsJsonAsync(httpClient, requestUri, value, cancellationToken);
                });
            }

            [TestMethod]
            public async Task SendsPostAsJson()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .Expect(HttpMethod.Post, "http://localhost/")
                    .WithHeaders("Content-Type", "application/json; charset=utf-8")
                    .WithContent("{\"myProp\":\"myValue\"}")
                    .Respond(HttpStatusCode.OK);
                var httpClient = new HttpClient(handler);
                var requestUri = "http://localhost/";
                var value = new { myProp = "myValue" };
                var cancellationToken = CancellationToken.None;

                // Act
                await HttpClientExtensions.PostAsJsonAsync(httpClient, requestUri, value, cancellationToken);

                // Assert
                handler.VerifyNoOutstandingExpectation();
            }
        }
    }
}
