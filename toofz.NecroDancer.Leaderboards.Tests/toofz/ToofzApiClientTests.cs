using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.Tests.Properties;
using toofz.NecroDancer.Leaderboards.toofz;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class ToofzApiClientTests
    {
        [TestClass]
        public class GetPlayersAsyncMethod
        {
            [TestMethod]
            public async Task ReturnsPlayersEnvelope()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://example.org/players")
                    .Respond("application/json", Resources.PlayersEnvelope);

                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };

                // Act
                var response = await toofzApiClient.GetPlayersAsync();

                // Assert
                Assert.AreEqual(453681, response.Total);
                Assert.AreEqual(20, response.Players.Count());
                var player = response.Players.First();
                Assert.AreEqual(76561198020278823, player.Id);
                Assert.AreEqual("Mr.moneybottoms", player.DisplayName);
                Assert.AreEqual(new DateTime(2017, 9, 13, 12, 48, 1, 350, DateTimeKind.Utc), player.UpdatedAt);
                Assert.AreEqual("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/cb/cb555a66da219db0dd0504b69ccbd810678fe203.jpg", player.Avatar);
            }
        }

        [TestClass]
        public class GetPlayersAsyncMethod_Params
        {
            [TestMethod]
            public async Task ReturnsPlayersEnvelope()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://example.org/players?limit=20&sort=updated_at")
                    .Respond("application/json", Resources.PlayersEnvelope);

                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };

                // Act
                var response = await toofzApiClient.GetPlayersAsync(new GetPlayersParams
                {
                    Limit = 20,
                    Sort = "updated_at",
                });

                // Assert
                Assert.AreEqual(453681, response.Total);
                Assert.AreEqual(20, response.Players.Count());
                var player = response.Players.First();
                Assert.AreEqual(76561198020278823, player.Id);
                Assert.AreEqual("Mr.moneybottoms", player.DisplayName);
                Assert.AreEqual(new DateTime(2017, 9, 13, 12, 48, 1, 350, DateTimeKind.Utc), player.UpdatedAt);
                Assert.AreEqual("https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/cb/cb555a66da219db0dd0504b69ccbd810678fe203.jpg", player.Avatar);
            }
        }

        [TestClass]
        public class PostPlayersAsyncMethod
        {
            [TestMethod]
            public async Task PlayersIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var toofzApiClient = new ToofzApiClient(handler, false);
                IEnumerable<Player> players = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return toofzApiClient.PostPlayersAsync(players);
                });
            }

            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://example.org/players")
                    .Respond("application/json", Resources.BulkStoreDTO);

                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };
                var players = new List<Player> { new Player { Exists = true, LastUpdate = new DateTime(2016, 1, 1) } };

                // Act
                var bulkStore = await toofzApiClient.PostPlayersAsync(players);

                // Assert
                Assert.AreEqual(10, bulkStore.RowsAffected);
            }
        }

        [TestClass]
        public class GetReplaysAsyncMethod
        {
            [TestMethod]
            public async Task ReturnsReplaysEnvelope()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://example.org/replays")
                    .Respond("application/json", Resources.ReplaysEnvelope);

                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };

                // Act
                var response = await toofzApiClient.GetReplaysAsync();

                // Assert
                Assert.AreEqual(43767, response.Total);
                Assert.AreEqual(20, response.Replays.Count());
                var replay = response.Replays.First();
                Assert.AreEqual(844845073340377377, replay.Id);
            }
        }

        [TestClass]
        public class GetReplaysAsyncMethod_Params
        {
            [TestMethod]
            public async Task ReturnsReplaysEnvelope()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://example.org/replays?limit=20")
                    .Respond("application/json", Resources.ReplaysEnvelope);

                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };

                // Act
                var response = await toofzApiClient.GetReplaysAsync(new GetReplaysParams
                {
                    Limit = 20,
                });

                // Assert
                Assert.AreEqual(43767, response.Total);
                Assert.AreEqual(20, response.Replays.Count());
                var replay = response.Replays.First();
                Assert.AreEqual(844845073340377377, replay.Id);
            }
        }

        [TestClass]
        public class PostReplaysAsyncMethod
        {
            [TestMethod]
            public async Task ReplaysIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };
                IEnumerable<Replay> replays = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return toofzApiClient.PostReplaysAsync(replays);
                });
            }

            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When("http://example.org/replays")
                    .Respond("application/json", Resources.BulkStoreDTO);

                var toofzApiClient = new ToofzApiClient(handler, false) { BaseAddress = new Uri("http://example.org/") };
                var replays = new List<Replay> { new Replay() };

                // Act
                var bulkStore = await toofzApiClient.PostReplaysAsync(replays);

                // Assert
                Assert.AreEqual(10, bulkStore.RowsAffected);
            }
        }
    }
}
