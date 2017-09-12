﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlBulkUpsert;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class LeaderboardsStoreClientTests
    {
        [TestClass]
        public class GetLeaderboardMappingsMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();

                // Act
                var mappings = storeClient.GetLeaderboardMappings();

                // Assert
                Assert.IsInstanceOfType(mappings, typeof(ColumnMappings<Leaderboard>));
            }
        }

        [TestClass]
        public class SaveChangesAsyncMethod_Leaderboards
        {
            [TestMethod]
            public async Task UpsertsLeaderboards()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Leaderboard>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var leaderboards = new List<Leaderboard>();

                // Act
                await storeClient.SaveChangesAsync(upserter, connection, leaderboards);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, leaderboards, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Leaderboard>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var leaderboards = new List<Leaderboard>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, leaderboards, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, connection, leaderboards);

                // Assert
                Assert.AreEqual(400, rowsAffected);
            }
        }

        [TestClass]
        public class GetEntryMappingsMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();

                // Act
                var mappings = storeClient.GetEntryMappings();

                // Assert
                Assert.IsInstanceOfType(mappings, typeof(ColumnMappings<Entry>));
            }
        }

        [TestClass]
        public class SaveChangesAsyncMethod_Entries
        {
            [TestMethod]
            public async Task InsertsEntries()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Entry>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var entries = new List<Entry>();

                // Act
                await storeClient.SaveChangesAsync(upserter, connection, entries);

                // Assert
                mockUpserter.Verify(u => u.InsertAsync(connection, entries, It.IsAny<CancellationToken>()), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Entry>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var entries = new List<Entry>();
                mockUpserter
                    .Setup(u => u.InsertAsync(connection, entries, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(20000));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, connection, entries);

                // Assert
                Assert.AreEqual(20000, rowsAffected);
            }
        }

        [TestClass]
        public class GetDailyLeaderboardMappingsMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();

                // Act
                var mappings = storeClient.GetDailyLeaderboardMappings();

                // Assert
                Assert.IsInstanceOfType(mappings, typeof(ColumnMappings<DailyLeaderboard>));
            }
        }

        [TestClass]
        public class SaveChangesAsyncMethod_DailyLeaderboards
        {
            [TestMethod]
            public async Task UpsertsDailyLeaderboards()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<DailyLeaderboard>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var dailyLeaderboards = new List<DailyLeaderboard>();

                // Act
                await storeClient.SaveChangesAsync(upserter, connection, dailyLeaderboards);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, dailyLeaderboards, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<DailyLeaderboard>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var dailyLeaderboards = new List<DailyLeaderboard>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, dailyLeaderboards, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, connection, dailyLeaderboards);

                // Assert
                Assert.AreEqual(400, rowsAffected);
            }
        }

        [TestClass]
        public class GetDailyEntryMappingsMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();

                // Act
                var mappings = storeClient.GetDailyEntryMappings();

                // Assert
                Assert.IsInstanceOfType(mappings, typeof(ColumnMappings<DailyEntry>));
            }
        }

        [TestClass]
        public class SaveChangesAsyncMethod_DailyEntries
        {
            [TestMethod]
            public async Task UpsertsDailyEntries()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<DailyEntry>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var dailyEntries = new List<DailyEntry>();

                // Act
                await storeClient.SaveChangesAsync(upserter, connection, dailyEntries);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, dailyEntries, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<DailyEntry>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var dailyEntries = new List<DailyEntry>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, dailyEntries, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, connection, dailyEntries);

                // Assert
                Assert.AreEqual(400, rowsAffected);
            }
        }

        [TestClass]
        public class GetPlayerMappingsMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();

                // Act
                var mappings = storeClient.GetPlayerMappings();

                // Assert
                Assert.IsInstanceOfType(mappings, typeof(ColumnMappings<Player>));
            }
        }

        [TestClass]
        public class SaveChangesAsyncMethod_Players
        {
            [TestMethod]
            public async Task UpsertsPlayers()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Player>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var players = new List<Player>();

                // Act
                await storeClient.SaveChangesAsync(upserter, connection, players, true);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, players, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Player>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var players = new List<Player>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, players, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, connection, players, true);

                // Assert
                Assert.AreEqual(400, rowsAffected);
            }
        }

        [TestClass]
        public class GetReplayMappingsMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();

                // Act
                var mappings = storeClient.GetReplayMappings();

                // Assert
                Assert.IsInstanceOfType(mappings, typeof(ColumnMappings<Replay>));
            }
        }

        [TestClass]
        public class SaveChangesAsyncMethod_Replays
        {
            [TestMethod]
            public async Task UpsertsReplays()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Replay>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var replays = new List<Replay>();

                // Act
                await storeClient.SaveChangesAsync(upserter, connection, replays, true);

                // Assert
                mockUpserter.Verify(u => u.UpsertAsync(connection, replays, true, It.IsAny<CancellationToken>()), Times.Once);
            }

            [TestMethod]
            public async Task ReturnsRowsAffected()
            {
                // Arrange
                var storeClient = new LeaderboardsStoreClient();
                var mockUpserter = new Mock<ITypedUpserter<Replay>>();
                var upserter = mockUpserter.Object;
                var connection = new SqlConnection();
                var replays = new List<Replay>();
                mockUpserter
                    .Setup(u => u.UpsertAsync(connection, replays, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(400));

                // Act
                var rowsAffected = await storeClient.SaveChangesAsync(upserter, connection, replays, true);

                // Assert
                Assert.AreEqual(400, rowsAffected);
            }
        }
    }
}