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
        public ToofzApiClientTests()
        {
            Client = new ToofzApiClient(Handler, false) { BaseAddress = new Uri("http://example.org/") };
        }

        public MockHttpMessageHandler Handler { get; set; } = new MockHttpMessageHandler();
        public ToofzApiClient Client { get; set; }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();

                // Act
                var client = new ToofzApiClient(handler, false);

                // Assert
                Assert.IsInstanceOfType(client, typeof(ToofzApiClient));
            }
        }

        [TestClass]
        public class GetPlayersAsyncMethod : ToofzApiClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetPlayersAsync();
                });
            }

            [TestMethod]
            public async Task ReturnsPlayersEnvelope()
            {
                // Arrange
                Handler.When("http://example.org/players").Respond("application/json", Resources.PlayersEnvelope);

                // Act
                var response = await Client.GetPlayersAsync();

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
        public class GetPlayersAsyncMethod_Params : ToofzApiClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                var @params = new GetPlayersParams
                {
                    Limit = 20,
                    Sort = "updated_at",
                };

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetPlayersAsync(@params);
                });
            }

            [TestMethod]
            public async Task ReturnsPlayersEnvelope()
            {
                // Arrange
                Handler.When("http://example.org/players?limit=20&sort=updated_at").Respond("application/json", Resources.PlayersEnvelope);
                var @params = new GetPlayersParams
                {
                    Limit = 20,
                    Sort = "updated_at",
                };

                // Act
                var response = await Client.GetPlayersAsync(@params);

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
        public class PostPlayersAsyncMethod : ToofzApiClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                var players = new List<Player>
                {
                    new Player
                    {
                        Exists = true,
                        LastUpdate = new DateTime(2016, 1, 1),
                    },
                };

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.PostPlayersAsync(players);
                });
            }

            [TestMethod]
            public async Task PlayersIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<Player> players = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return Client.PostPlayersAsync(players);
                });
            }

            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                Handler.When("http://example.org/players").Respond("application/json", Resources.BulkStoreDTO);
                var players = new List<Player>
                {
                    new Player
                    {
                        Exists = true,
                        LastUpdate = new DateTime(2016, 1, 1),
                    },
                };

                // Act
                var bulkStore = await Client.PostPlayersAsync(players);

                // Assert
                Assert.AreEqual(10, bulkStore.RowsAffected);
            }
        }

        [TestClass]
        public class GetReplaysAsyncMethod : ToofzApiClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetReplaysAsync();
                });
            }

            [TestMethod]
            public async Task ReturnsReplaysEnvelope()
            {
                // Arrange
                Handler.When("http://example.org/replays").Respond("application/json", Resources.ReplaysEnvelope);

                // Act
                var response = await Client.GetReplaysAsync();

                // Assert
                Assert.AreEqual(43767, response.Total);
                Assert.AreEqual(20, response.Replays.Count());
                var replay = response.Replays.First();
                Assert.AreEqual(844845073340377377, replay.Id);
            }
        }

        [TestClass]
        public class GetReplaysAsyncMethod_Params : ToofzApiClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                var @params = new GetReplaysParams
                {
                    Limit = 20,
                };

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.GetReplaysAsync(@params);
                });
            }

            [TestMethod]
            public async Task ReturnsReplaysEnvelope()
            {
                // Arrange
                Handler.When("http://example.org/replays?limit=20").Respond("application/json", Resources.ReplaysEnvelope);
                var @params = new GetReplaysParams
                {
                    Limit = 20,
                };

                // Act
                var response = await Client.GetReplaysAsync(@params);

                // Assert
                Assert.AreEqual(43767, response.Total);
                Assert.AreEqual(20, response.Replays.Count());
                var replay = response.Replays.First();
                Assert.AreEqual(844845073340377377, replay.Id);
            }
        }

        [TestClass]
        public class PostReplaysAsyncMethod : ToofzApiClientTests
        {
            [TestMethod]
            public async Task Disposed_ThrowsObjectDisposedException()
            {
                // Arrange
                Client.Dispose();
                var replays = new List<Replay>
                {
                    new Replay(),
                };

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ObjectDisposedException>(() =>
                {
                    return Client.PostReplaysAsync(replays);
                });
            }

            [TestMethod]
            public async Task ReplaysIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<Replay> replays = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return Client.PostReplaysAsync(replays);
                });
            }

            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                Handler.When("http://example.org/replays").Respond("application/json", Resources.BulkStoreDTO);
                var replays = new List<Replay>
                {
                    new Replay(),
                };

                // Act
                var bulkStore = await Client.PostReplaysAsync(replays);

                // Assert
                Assert.AreEqual(10, bulkStore.RowsAffected);
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
                var client = new ToofzApiClient(handler, true);

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
                var client = new ToofzApiClient(handler, true);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                Assert.AreEqual(1, handler.DisposeCount);
            }
        }
    }
}
