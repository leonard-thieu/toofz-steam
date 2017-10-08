﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Steam.CommunityData;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.CommunityData
{
    class SteamCommunityDataClientTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = Mock.Of<HttpMessageHandler>();

                // Act
                var client = new SteamCommunityDataClient(handler);

                // Assert
                Assert.IsInstanceOfType(client, typeof(SteamCommunityDataClient));
            }
        }

        [TestClass]
        public class DefaultRequestHeadersProperty
        {
            [TestMethod]
            public void ReturnsDefaultRequestHeaders()
            {
                // Arrange
                var handler = Mock.Of<HttpMessageHandler>();
                var client = new SteamCommunityDataClient(handler);

                // Act
                var defaultRequestHeaders = client.DefaultRequestHeaders;

                // Assert
                Assert.IsInstanceOfType(defaultRequestHeaders, typeof(HttpRequestHeaders));
            }
        }

        [TestClass]
        public class GetLeaderboardsAsyncMethod_UInt32
        {
            [TestMethod]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/?xml=1")
                    .Respond(new StringContent(CommunityDataResources.Leaderboards, Encoding.UTF8, "text/xml"));
                var client = new SteamCommunityDataClient(handler);
                var appId = 247080U;

                // Act
                var leaderboardsEnvelope = await client.GetLeaderboardsAsync(appId);

                // Assert
                Assert.AreEqual(411, leaderboardsEnvelope.Leaderboards.Count);
                var leaderboard = leaderboardsEnvelope.Leaderboards.First();
                Assert.AreEqual("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1", leaderboard.Url);
                Assert.AreEqual(2047387, leaderboard.LeaderboardId);
                Assert.AreEqual("DLC HARDCORE All Chars DLC_PROD", leaderboard.Name);
                Assert.AreEqual("All Characters (DLC) Score (Amplified)", leaderboard.DisplayName);
                Assert.AreEqual(317, leaderboard.EntryCount);
                Assert.AreEqual(2, leaderboard.SortMethod);
                Assert.AreEqual(1, leaderboard.DisplayType);
            }
        }

        [TestClass]
        public class GetLeaderboardsAsyncMethod_String
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                var handler = Mock.Of<HttpMessageHandler>();
                var client = new SteamCommunityDataClient(handler);
                client.Dispose();
                var communityGameName = 247080U.ToString();

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return client.GetLeaderboardsAsync(communityGameName);
                });
            }

            [TestMethod]
            public async Task CommunityGameNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = Mock.Of<HttpMessageHandler>();
                var client = new SteamCommunityDataClient(handler);
                string communityGameName = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return client.GetLeaderboardsAsync(communityGameName);
                });
            }

            [TestMethod]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/?xml=1")
                    .Respond(new StringContent(CommunityDataResources.Leaderboards, Encoding.UTF8, "text/xml"));
                var client = new SteamCommunityDataClient(handler);
                var communityGameName = 247080U.ToString();

                // Act
                var leaderboardsEnvelope = await client.GetLeaderboardsAsync(communityGameName);

                // Assert
                Assert.AreEqual(411, leaderboardsEnvelope.Leaderboards.Count);
                var leaderboard = leaderboardsEnvelope.Leaderboards.First();
                Assert.AreEqual("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1", leaderboard.Url);
                Assert.AreEqual(2047387, leaderboard.LeaderboardId);
                Assert.AreEqual("DLC HARDCORE All Chars DLC_PROD", leaderboard.Name);
                Assert.AreEqual("All Characters (DLC) Score (Amplified)", leaderboard.DisplayName);
                Assert.AreEqual(317, leaderboard.EntryCount);
                Assert.AreEqual(2, leaderboard.SortMethod);
                Assert.AreEqual(1, leaderboard.DisplayType);
            }
        }

        [TestClass]
        public class GetLeaderboardEntriesAsyncMethod_UInt32
        {
            [TestMethod]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(CommunityDataResources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var client = new SteamCommunityDataClient(handler);
                var appId = 247080U;
                var leaderboardId = 2047387;

                // Act
                var leaderboardEntriesEnvelope = await client.GetLeaderboardEntriesAsync(appId, leaderboardId);

                // Assert
                var entries = leaderboardEntriesEnvelope.Entries;
                Assert.AreEqual(317, entries.Count);
                var entry = entries.First();
                Assert.AreEqual(76561197998799529, entry.SteamId);
                Assert.AreEqual(134377, entry.Score);
                Assert.AreEqual(1, entry.Rank);
                Assert.AreEqual(849347241492683863UL, entry.UgcId);
                Assert.AreEqual("0b00000001000000", entry.Details);
            }
        }

        [TestClass]
        public class GetLeaderboardEntriesAsyncMethod_String
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                var handler = Mock.Of<HttpMessageHandler>();
                var client = new SteamCommunityDataClient(handler);
                client.Dispose();
                var communityGameName = 247080U.ToString();
                var leaderboardId = 2047387;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return client.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);
                });
            }

            [TestMethod]
            public async Task CommunityGameNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = Mock.Of<HttpMessageHandler>();
                var client = new SteamCommunityDataClient(handler);
                string communityGameName = null;
                var leaderboardId = 2047387;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return client.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);
                });
            }

            [TestMethod]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://steamcommunity.com/stats/247080/leaderboards/2047387/?xml=1")
                    .Respond(new StringContent(CommunityDataResources.LeaderboardEntries, Encoding.UTF8, "text/xml"));
                var client = new SteamCommunityDataClient(handler);
                var communityGameName = 247080U.ToString();
                var leaderboardId = 2047387;

                // Act
                var leaderboardEntriesEnvelope = await client.GetLeaderboardEntriesAsync(communityGameName, leaderboardId);

                // Assert
                var entries = leaderboardEntriesEnvelope.Entries;
                Assert.AreEqual(317, entries.Count);
                var entry = entries.First();
                Assert.AreEqual(76561197998799529, entry.SteamId);
                Assert.AreEqual(134377, entry.Score);
                Assert.AreEqual(1, entry.Rank);
                Assert.AreEqual(849347241492683863UL, entry.UgcId);
                Assert.AreEqual("0b00000001000000", entry.Details);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void DisposesHttpClient()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new SteamCommunityDataClient(handler);

                // Act
                client.Dispose();

                // Assert
                Assert.AreEqual(1, handler.DisposeCount);
            }

            [TestMethod]
            public void DisposeMoreThanOnce_OnlyDisposesHttpClientOnce()
            {
                // Arrange
                var handler = new SimpleHttpMessageHandler();
                var client = new SteamCommunityDataClient(handler);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.AreEqual(1, handler.DisposeCount);
            }

            class SimpleHttpMessageHandler : HttpMessageHandler
            {
                protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                {
                    throw new NotImplementedException();
                }

                public int DisposeCount { get; private set; }

                protected override void Dispose(bool disposing)
                {
                    DisposeCount++;
                    base.Dispose(disposing);
                }
            }
        }
    }
}