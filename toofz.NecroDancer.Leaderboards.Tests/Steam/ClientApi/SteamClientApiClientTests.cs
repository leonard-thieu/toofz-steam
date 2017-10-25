using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using static SteamKit2.SteamUserStats;

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
            public void ReturnsInstance()
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
            public void ReturnsProgress()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.ProgressDebugNetworkListener).Returns(new ProgressDebugNetworkListener { Progress = Mock.Of<IProgress<long>>() });
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

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
            public void SetsProgress()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.ProgressDebugNetworkListener).Returns(new ProgressDebugNetworkListener());
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);
                var progress = Mock.Of<IProgress<long>>();

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
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(), Times.Once);
            }

            [TestMethod]
            public async Task ConnectedToSteam_DoesNotConnectToSteam()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(), Times.Never);
            }

            [TestMethod]
            public async Task NotLoggedOnToSteam_LogsOnToSteam()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

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
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

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
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

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
                mockLeaderboardCallback = new Mock<IFindOrCreateLeaderboardCallback>();
                var leaderboardCallback = mockLeaderboardCallback.Object;
                mockLeaderboardResponse = new Mock<IAsyncJob<IFindOrCreateLeaderboardCallback>>();
                mockLeaderboardResponse.Setup(r => r.ToTask()).Returns(Task.FromResult(leaderboardCallback));
                var leaderboardResponse = mockLeaderboardResponse.Object;

                mockSteamUserStats = new Mock<ISteamUserStats>();
                mockSteamUserStats
                    .Setup(s => s.FindLeaderboard(It.IsAny<uint>(), It.IsAny<string>()))
                    .Returns(leaderboardResponse);
                var steamUserStats = mockSteamUserStats.Object;

                mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
                mockSteamClient.Setup(c => c.GetSteamUserStats()).Returns(steamUserStats);
                var steamClient = mockSteamClient.Object;

                steamClientApiClient = new SteamClientApiClient(UserName, Password, steamClient);
            }

            Mock<IFindOrCreateLeaderboardCallback> mockLeaderboardCallback;
            Mock<IAsyncJob<IFindOrCreateLeaderboardCallback>> mockLeaderboardResponse;
            Mock<ISteamUserStats> mockSteamUserStats;
            Mock<ISteamClientAdapter> mockSteamClient;
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
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.Fail);

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
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                Assert.IsInstanceOfType(leaderboardEntries, typeof(IFindOrCreateLeaderboardCallback));
            }

            [TestMethod]
            public async Task SetsTimeout()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                mockLeaderboardResponse.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
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
                var leaderboardEntriesCallback = mockLeaderboardEntriesCallback.Object;
                mockLeaderboardEntriesResponse = new Mock<IAsyncJob<ILeaderboardEntriesCallback>>();
                mockLeaderboardEntriesResponse.Setup(r => r.ToTask()).Returns(Task.FromResult(leaderboardEntriesCallback));
                var leaderboardEntriesResponse = mockLeaderboardEntriesResponse.Object;

                mockSteamUserStats = new Mock<ISteamUserStats>();
                mockSteamUserStats
                    .Setup(s => s.GetLeaderboardEntries(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ELeaderboardDataRequest>()))
                    .Returns(leaderboardEntriesResponse);
                var steamUserStats = mockSteamUserStats.Object;

                mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
                mockSteamClient.Setup(c => c.GetSteamUserStats()).Returns(steamUserStats);
                var steamClient = mockSteamClient.Object;

                steamClientApiClient = new SteamClientApiClient(UserName, Password, steamClient);
            }

            Mock<ILeaderboardEntriesCallback> mockLeaderboardEntriesCallback;
            Mock<IAsyncJob<ILeaderboardEntriesCallback>> mockLeaderboardEntriesResponse;
            Mock<ISteamUserStats> mockSteamUserStats;
            Mock<ISteamClientAdapter> mockSteamClient;
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
                mockLeaderboardEntriesResponse.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void DisposesSteamClient()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

                // Act
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Dispose(), Times.Once);
            }

            [TestMethod]
            public void DisposeMoreThanOnce_DoesNothing()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, steamClient);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Never);
            }
        }
    }
}
