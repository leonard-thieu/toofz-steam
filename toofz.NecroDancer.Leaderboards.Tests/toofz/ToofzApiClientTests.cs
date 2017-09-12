using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests.toofz
{
    class ToofzApiClientTests
    {
        [TestClass]
        public class GetPlayersAsyncMethod
        {
            [TestMethod]
            public async Task ReturnsPlayersDTO()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "players?limit=20&sort=updated_at"))
                    .RespondJson(new PlayersEnvelope());

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };

                // Act
                var players = await toofzApiClient.GetPlayersAsync(new GetPlayersParams
                {
                    Limit = 20,
                    Sort = "updated_at",

                });

                // Assert
                Assert.IsInstanceOfType(players, typeof(PlayersEnvelope));
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
                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };
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
                    .When(new Uri(Constants.FakeUri + "players"))
                    .RespondJson(new BulkStoreDTO());

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };
                var players = new List<Player> { new Player { Exists = true, LastUpdate = new DateTime(2016, 1, 1) } };

                // Act
                var bulkStore = await toofzApiClient.PostPlayersAsync(players);

                // Assert
                Assert.IsInstanceOfType(bulkStore, typeof(BulkStoreDTO));
            }
        }

        [TestClass]
        public class GetReplaysAsyncMethod
        {
            [TestMethod]
            public async Task ReturnsReplaysDTO()
            {
                // Arrange
                var handler = new MockHttpMessageHandler();
                handler
                    .When(new Uri(Constants.FakeUri + "replays?limit=20"))
                    .RespondJson(new ReplaysEnvelope());

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };

                // Act
                var replayIds = await toofzApiClient.GetReplaysAsync(new GetReplaysParams
                {
                    Limit = 20,
                });

                // Assert
                Assert.IsInstanceOfType(replayIds, typeof(ReplaysEnvelope));
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
                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };
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
                    .When(new Uri(Constants.FakeUri + "replays"))
                    .RespondJson(new BulkStoreDTO());

                var toofzApiClient = new ToofzApiClient(handler) { BaseAddress = Constants.FakeUri };
                var replays = new List<Replay> { new Replay() };

                // Act
                var bulkStore = await toofzApiClient.PostReplaysAsync(replays);

                // Assert
                Assert.IsInstanceOfType(bulkStore, typeof(BulkStoreDTO));
            }
        }
    }
}
