using System;
using System.Threading.Tasks;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;
using static SteamKit2.SteamUserStats;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientApiClientTests
    {
        public class Constructor
        {
            [Fact]
            public void UserNameIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = null;
                string password = "password";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [Fact]
            public void UserNameIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "";
                string password = "password";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [Fact]
            public void PasswordIsNull_ThrowsArgumentException()
            {
                // Arrange
                string userName = "userName";
                string password = null;

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [Fact]
            public void PasswordIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string userName = "userName";
                string password = "";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                string userName = "userName";
                string password = "password";

                // Act
                var client = new SteamClientApiClient(userName, password);

                // Assert
                Assert.IsAssignableFrom<SteamClientApiClient>(client);
            }
        }

        public class ProgressGetter
        {
            [Fact]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var client = new SteamClientApiClient(userName, password);
                client.Dispose();

                // Act -> Assert
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    client.Progress.ToString();
                });
            }

            [Fact]
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
                Assert.IsAssignableFrom<IProgress<long>>(progress);
            }
        }

        public class ProgressSetter
        {
            [Fact]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                string userName = "userName";
                string password = "password";
                var client = new SteamClientApiClient(userName, password);
                client.Dispose();

                // Act -> Assert
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    client.Progress = null;
                });
            }

            [Fact]
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
                Assert.IsAssignableFrom<IProgress<long>>(client.Progress);
            }
        }

        public class TimeoutProperty
        {
            [Fact]
            public void ReturnsDefault()
            {
                // Arrange
                var userName = "myUserName";
                var password = "myPassword";
                var client = new SteamClientApiClient(userName, password);

                // Act
                var timeout = client.Timeout;

                // Assert
                Assert.Equal(TimeSpan.FromSeconds(10), timeout);
            }

            [Fact]
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
                Assert.Equal(timeout, timeout2);
            }
        }

        public class ConnectAndLogOnAsyncMethod
        {
            [Fact]
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

            [Fact]
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

            [Fact]
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

            [Fact]
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

        public class DisconnectMethod
        {
            [Fact]
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

        public class FindLeaderboardAsyncMethod
        {
            private const string UserName = "userName";
            private const string Password = "password";
            private const uint AppId = 247080;
            private const string LeaderboardName = "Leaderboard Name";

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

            private Mock<IFindOrCreateLeaderboardCallback> mockLeaderboardCallback;
            private Mock<IAsyncJob<IFindOrCreateLeaderboardCallback>> mockLeaderboardResponse;
            private Mock<ISteamUserStats> mockSteamUserStats;
            private Mock<ISteamClientAdapter> mockSteamClient;
            private SteamClientApiClient steamClientApiClient;

            [Fact]
            public async Task NotConnected_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
            }

            [Fact]
            public async Task NotLoggedOn_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
            }

            [Fact]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.Fail);

                // Act -> Assert
                var ex = await Assert.ThrowsAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
                Assert.Equal(EResult.Fail, ex.Result);
            }

            [Fact]
            public async Task IDIsZero_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);
                mockLeaderboardCallback.Setup(le => le.ID).Returns(0);

                // Act -> Assert
                await Assert.ThrowsAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);
                });
            }

            [Fact]
            public async Task ResultIsOK_ReturnsLeaderboardEntriesCallback()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);
                mockLeaderboardCallback.Setup(le => le.ID).Returns(1);

                // Act
                var leaderboardEntries = await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                Assert.IsAssignableFrom<IFindOrCreateLeaderboardCallback>(leaderboardEntries);
            }

            [Fact]
            public async Task SetsTimeout()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);
                mockLeaderboardCallback.Setup(le => le.ID).Returns(1);

                // Act
                await steamClientApiClient.FindLeaderboardAsync(AppId, LeaderboardName);

                // Assert
                mockLeaderboardResponse.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
            }
        }

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

            [Fact]
            public async Task NotConnected_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });
            }

            [Fact]
            public async Task NotLoggedOn_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });
            }

            [Fact]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.Fail);

                // Act -> Assert
                var ex = await Assert.ThrowsAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);
                });
                Assert.Equal(EResult.Fail, ex.Result);
            }

            [Fact]
            public async Task ResultIsOK_ReturnsLeaderboardEntriesCallback()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.GetLeaderboardEntriesAsync(AppId, LeaderboardId);

                // Assert
                Assert.IsAssignableFrom<ILeaderboardEntriesCallback>(leaderboardEntries);
            }

            [Fact]
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

        public class DisposeMethod
        {
            [Fact]
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

            [Fact]
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
