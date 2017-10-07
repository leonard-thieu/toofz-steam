using System;
using System.Net;
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
        public class ProgressGetter
        {
            [TestMethod]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var client = new SteamClientApiClient(userName, password);
                client.Dispose();

                // Act -> Assert
                Assert.ThrowsException<ObjectDisposedException>(() =>
                {
                    client.Progress.ToString();
                });
            }

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
        }

        [TestClass]
        public class ProgressSetter
        {
            [TestMethod]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var client = new SteamClientApiClient(userName, password);
                client.Dispose();

                // Act -> Assert
                Assert.ThrowsException<ObjectDisposedException>(() =>
                {
                    client.Progress = null;
                });
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
        public class TimeoutProperty
        {
            [TestMethod]
            public void ReturnsDefault()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var client = new SteamClientApiClient(userName, password);

                // Act
                var timeout = client.Timeout;

                // Assert
                Assert.AreEqual(TimeSpan.FromSeconds(10), timeout);
            }

            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var client = new SteamClientApiClient(userName, password);
                var timeout = TimeSpan.FromSeconds(1);

                // Act
                client.Timeout = timeout;
                var timeout2 = client.Timeout;

                // Assert
                Assert.AreEqual(timeout, timeout2);
            }
        }

        [TestClass]
        public class ConnectAndLogOnAsyncMethod
        {
            [TestMethod]
            public async Task NotConnectedToSteam_ConnectsToSteam()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var mockManager = new Mock<ICallbackManager>();
                var mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);
                var steamClient = mockSteamClient.Object;
                mockManager.SetupGet(m => m.SteamClient).Returns(steamClient);
                var manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(It.IsAny<IPEndPoint>()), Times.Once);
            }

            [TestMethod]
            public async Task ConnectedToSteam_DoesNotConnectToSteam()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var mockManager = new Mock<ICallbackManager>();
                var mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                var steamClient = mockSteamClient.Object;
                mockManager.SetupGet(m => m.SteamClient).Returns(steamClient);
                var manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(It.IsAny<IPEndPoint>()), Times.Never);
            }

            [TestMethod]
            public async Task NotLoggedOnToSteam_LogsOnToSteam()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var mockManager = new Mock<ICallbackManager>();
                var mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);
                var steamClient = mockSteamClient.Object;
                mockManager.SetupGet(m => m.SteamClient).Returns(steamClient);
                var manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.LogOnAsync(It.IsAny<SteamUser.LogOnDetails>()), Times.Once);
            }

            [TestMethod]
            public async Task LoggedOnToSteam_DoesNotLogOnToSteam()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var mockManager = new Mock<ICallbackManager>();
                var mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
                var steamClient = mockSteamClient.Object;
                mockManager.SetupGet(m => m.SteamClient).Returns(steamClient);
                var manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.LogOnAsync(It.IsAny<SteamUser.LogOnDetails>()), Times.Never);
            }
        }

        [TestClass]
        public class DisconnectMethod
        {
            [TestMethod]
            public void DisconnectsFromSteam()
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
                client.Disconnect();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Once);
            }
        }

        [TestClass]
        public class FindLeaderboardAsyncMethod
        {
            const string UserName = "userName";
            const string Password = "password";
            const uint AppId = 247080;
            const string LeaderboardName = "Leaderboard Name";

            public FindLeaderboardAsyncMethod()
            {
                mockFindOrCreateLeaderboardCallback = new Mock<IFindOrCreateLeaderboardCallback>();

                mockSteamUserStats = new Mock<ISteamUserStats>();
                mockSteamUserStats
                    .Setup(s => s.FindLeaderboard(It.IsAny<uint>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(mockFindOrCreateLeaderboardCallback.Object));

                mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.Setup(c => c.GetSteamUserStats()).Returns(mockSteamUserStats.Object);
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);

                mockManager = new Mock<ICallbackManager>();
                mockManager.SetupGet(m => m.SteamClient).Returns(mockSteamClient.Object);

                steamClientApiClient = new SteamClientApiClient(UserName, Password, mockManager.Object);
            }

            Mock<IFindOrCreateLeaderboardCallback> mockFindOrCreateLeaderboardCallback;
            Mock<ISteamUserStats> mockSteamUserStats;
            Mock<ISteamClient> mockSteamClient;
            Mock<ICallbackManager> mockManager;
            SteamClientApiClient steamClientApiClient;

            [TestMethod]
            public async Task NotConnected_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
            }

            [TestMethod]
            public async Task NotLoggedOn_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
            }

            [TestMethod]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockFindOrCreateLeaderboardCallback.Setup(le => le.Result).Returns(EResult.Fail);

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
                mockFindOrCreateLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(IFindOrCreateLeaderboardCallback));
            }

            [TestMethod]
            public async Task SetsTimeout()
            {
                // Arrange
                mockFindOrCreateLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                mockSteamUserStats.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
            }
        }

        [TestClass]
        public class GetLeaderboardEntriesAsyncMethod
        {
            const string UserName = "userName";
            const string Password = "password";
            const uint AppId = 247080;
            const int LeaderboardId = 739999;

            public GetLeaderboardEntriesAsyncMethod()
            {
                mockLeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();

                mockSteamUserStats = new Mock<ISteamUserStats>();
                mockSteamUserStats
                    .Setup(s => s.GetLeaderboardEntries(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ELeaderboardDataRequest>()))
                    .Returns(Task.FromResult(mockLeaderboardEntriesCallback.Object));

                mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.Setup(c => c.GetSteamUserStats()).Returns(mockSteamUserStats.Object);
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);

                mockmanager = new Mock<ICallbackManager>();
                mockmanager.SetupGet(manager => manager.SteamClient).Returns(mockSteamClient.Object);

                steamClientApiClient = new SteamClientApiClient(UserName, Password, mockmanager.Object);
            }

            Mock<ILeaderboardEntriesCallback> mockLeaderboardEntriesCallback;
            Mock<ISteamUserStats> mockSteamUserStats;
            Mock<ISteamClient> mockSteamClient;
            Mock<ICallbackManager> mockmanager;
            SteamClientApiClient steamClientApiClient;

            [TestMethod]
            public async Task NotConnected_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });
            }

            [TestMethod]
            public async Task NotLoggedOn_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });
            }

            [TestMethod]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.Fail);

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
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(ILeaderboardEntriesCallback));
            }

            [TestMethod]
            public async Task SetsTimeout()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                await steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);

                // Assert
                mockSteamUserStats.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void IsConnected_DisconnectsFromSteam()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(true);
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Once);
            }

            [TestMethod]
            public void IsNotConnected_DoesNotDisconnectFromSteam()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(false);
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Never);
            }

            [TestMethod]
            public void DisposeMoreThanOnce_DoesNothing()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(false);
                Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
                mockManager
                    .Setup(m => m.SteamClient)
                    .Returns(mockSteamClient.Object);
                ICallbackManager manager = mockManager.Object;
                var client = new SteamClientApiClient(userName, password, manager);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Never);
            }
        }
    }
}
