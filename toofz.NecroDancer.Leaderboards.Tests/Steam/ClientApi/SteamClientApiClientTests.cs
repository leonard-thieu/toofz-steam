using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    class SteamClientApiClientTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void UserNameIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = null;
                string password = "password";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [TestMethod]
            public void UserNameIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "";
                string password = "password";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [TestMethod]
            public void PasswordIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = "userName";
                string password = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [TestMethod]
            public void PasswordIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "userName";
                string password = "";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [TestMethod]
            public void ReturnsSteamClientApiClient()
            {
                // Arrange
                string userName = "userName";
                string password = "password";

                // Act
                var client = new SteamClientApiClient(userName, password);

                // Assert
                Assert.IsInstanceOfType(client, typeof(SteamClientApiClient));
            }
        }

        [TestClass]
        public class ProgressProperty
        {
            [TestMethod]
            public void ProgressDebugNetworkListenerIsNull_ReturnsNull()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                var progress = client.Progress;

                // Assert
                Assert.IsNull(progress);
            }

            [TestMethod]
            public void ProgressDebugNetworkListenerIsNotNullAndProgressIsNull_ReturnsNull()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient
                    .Setup(c => c.ProgressDebugNetworkListener)
                    .Returns(new ProgressDebugNetworkListener());
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                var progress = client.Progress;

                // Assert
                Assert.IsNull(progress);
            }

            [TestMethod]
            public void ProgressDebugNetworkListenerIsNotNullAndProgressIsNotNull_ReturnsInstance()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient
                    .Setup(c => c.ProgressDebugNetworkListener)
                    .Returns(new ProgressDebugNetworkListener { Progress = Mock.Of<IProgress<long>>() });
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                var progress = client.Progress;

                // Assert
                Assert.IsInstanceOfType(progress, typeof(IProgress<long>));
            }

            [TestMethod]
            public void ProgressDebugNetworkListenerIsNull_ThrowsInvalidOperationException()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act -> Assert
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    client.Progress = Mock.Of<IProgress<long>>();
                });
            }

            [TestMethod]
            public void SetsProgress()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient
                    .Setup(c => c.ProgressDebugNetworkListener)
                    .Returns(new ProgressDebugNetworkListener());
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);
                IProgress<long> progress = Mock.Of<IProgress<long>>();

                // Act
                client.Progress = progress;

                // Assert
                Assert.IsInstanceOfType(client.Progress, typeof(IProgress<long>));
            }
        }

        [TestClass]
        public class FindLeaderboardAsync
        {
            const string UserName = "userName";
            const string Password = "password";
            const uint AppId = 247080;
            const string LeaderboardName = "Leaderboard Name";

            Mock<IFindOrCreateLeaderboardCallback> mockIFindOrCreateLeaderboardCallback;
            Mock<ISteamUserStats> mockISteamUserStats;
            Mock<ISteamClient> mockISteamClient;
            Mock<ICallbackManager> mockICallbackManager;
            SteamClientApiClient steamClientApiClient;

            [TestInitialize]
            public void TestInitialize()
            {
                mockIFindOrCreateLeaderboardCallback = new Mock<IFindOrCreateLeaderboardCallback>();

                mockISteamUserStats = new Mock<ISteamUserStats>();
                mockISteamUserStats
                    .Setup(s => s.FindLeaderboard(It.IsAny<uint>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(mockIFindOrCreateLeaderboardCallback.Object));

                mockISteamClient = new Mock<ISteamClient>();
                mockISteamClient
                    .Setup(c => c.GetSteamUserStats())
                    .Returns(mockISteamUserStats.Object);

                mockICallbackManager = new Mock<ICallbackManager>();
                mockICallbackManager
                    .SetupGet(manager => manager.SteamClient)
                    .Returns(mockISteamClient.Object);

                steamClientApiClient = new SteamClientApiClient(UserName, Password, mockICallbackManager.Object);
            }

            [TestMethod]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockIFindOrCreateLeaderboardCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.Fail);

                // Act -> Assert
                var ex = await Assert.ThrowsExceptionAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
                Assert.AreEqual(EResult.Fail, ex.Result);
            }

            [TestMethod]
            public async Task ResultIsOK_ReturnsLeaderboardEntriesCallback()
            {
                // Arrange
                mockIFindOrCreateLeaderboardCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(IFindOrCreateLeaderboardCallback));
            }
        }

        [TestClass]
        public class GetLeaderboardEntriesAsync
        {
            const string UserName = "userName";
            const string Password = "password";
            const uint AppId = 247080;
            const int LeaderboardId = 739999;

            Mock<ILeaderboardEntriesCallback> mockILeaderboardEntriesCallback;
            Mock<ISteamUserStats> mockISteamUserStats;
            Mock<ISteamClient> mockISteamClient;
            Mock<ICallbackManager> mockICallbackManager;
            SteamClientApiClient steamClientApiClient;

            [TestInitialize]
            public void TestInitialize()
            {
                mockILeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();

                mockISteamUserStats = new Mock<ISteamUserStats>();
                mockISteamUserStats
                    .Setup(s => s.GetLeaderboardEntries(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ELeaderboardDataRequest>()))
                    .Returns(Task.FromResult(mockILeaderboardEntriesCallback.Object));

                mockISteamClient = new Mock<ISteamClient>();
                mockISteamClient
                    .Setup(c => c.GetSteamUserStats())
                    .Returns(mockISteamUserStats.Object);

                mockICallbackManager = new Mock<ICallbackManager>();
                mockICallbackManager
                    .SetupGet(manager => manager.SteamClient)
                    .Returns(mockISteamClient.Object);

                steamClientApiClient = new SteamClientApiClient(UserName, Password, mockICallbackManager.Object);
            }

            [TestMethod]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockILeaderboardEntriesCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.Fail);

                // Act -> Assert
                var ex = await Assert.ThrowsExceptionAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });
                Assert.AreEqual(EResult.Fail, ex.Result);
            }

            [TestMethod]
            public async Task ResultIsOK_ReturnsLeaderboardEntriesCallback()
            {
                // Arrange
                mockILeaderboardEntriesCallback
                    .Setup(le => le.Result)
                    .Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(ILeaderboardEntriesCallback));
            }
        }
    }
}
