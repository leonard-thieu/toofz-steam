using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;
using static SteamKit2.SteamUserStats;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientApiClientTests
    {
        private string userName = "myUserName";
        private string password = "myPassword";
        private TelemetryClient telemetryClient = new TelemetryClient();

        public class Constructor : SteamClientApiClientTests
        {
            [Fact]
            public void UserNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                userName = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientApiClient(userName, password, telemetryClient);
                });
            }

            [Fact]
            public void UserNameIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                userName = "";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password, telemetryClient);
                });
            }

            [Fact]
            public void PasswordIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                password = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientApiClient(userName, password, telemetryClient);
                });
            }

            [Fact]
            public void PasswordIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                password = "";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password, telemetryClient);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var client = new SteamClientApiClient(userName, password, telemetryClient);

                // Assert
                Assert.IsAssignableFrom<SteamClientApiClient>(client);
            }
        }

        public class ProgressGetter : SteamClientApiClientTests
        {
            [Fact]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                var client = new SteamClientApiClient(userName, password, telemetryClient);
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
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.ProgressDebugNetworkListener).Returns(new ProgressDebugNetworkListener { Progress = Mock.Of<IProgress<long>>() });
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                var progress = client.Progress;

                // Assert
                Assert.IsAssignableFrom<IProgress<long>>(progress);
            }
        }

        public class ProgressSetter : SteamClientApiClientTests
        {
            [Fact]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                var client = new SteamClientApiClient(userName, password, telemetryClient);
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
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.ProgressDebugNetworkListener).Returns(new ProgressDebugNetworkListener());
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);
                var progress = Mock.Of<IProgress<long>>();

                // Act
                client.Progress = progress;

                // Assert
                Assert.IsAssignableFrom<IProgress<long>>(client.Progress);
            }
        }

        public class TimeoutProperty : SteamClientApiClientTests
        {
            [Fact]
            public void ReturnsDefault()
            {
                // Arrange
                var client = new SteamClientApiClient(userName, password, telemetryClient);

                // Act
                var timeout = client.Timeout;

                // Assert
                Assert.Equal(TimeSpan.FromSeconds(10), timeout);
            }

            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var client = new SteamClientApiClient(userName, password, telemetryClient);
                var timeout = TimeSpan.FromSeconds(1);

                // Act
                client.Timeout = timeout;
                var timeout2 = client.Timeout;

                // Assert
                Assert.Equal(timeout, timeout2);
            }
        }

        public class ConnectAndLogOnAsyncMethod : SteamClientApiClientTests
        {
            [Fact]
            public async Task NotConnectedToSteam_ConnectsToSteam()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(), Times.Once);
            }

            [Fact]
            public async Task ConnectedToSteam_DoesNotConnectToSteam()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(), Times.Never);
            }

            [Fact]
            public async Task NotLoggedOnToSteam_LogsOnToSteam()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.LogOnAsync(It.IsAny<SteamUser.LogOnDetails>()), Times.Once);
            }

            [Fact]
            public async Task LoggedOnToSteam_DoesNotLogOnToSteam()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                await client.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.LogOnAsync(It.IsAny<SteamUser.LogOnDetails>()), Times.Never);
            }
        }

        public class DisconnectMethod : SteamClientApiClientTests
        {
            [Fact]
            public void DisconnectsFromSteam()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                client.Disconnect();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Once);
            }
        }

        public class FindLeaderboardAsyncMethod : SteamClientApiClientTests
        {
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

                steamClientApiClient = new SteamClientApiClient(userName, password, telemetryClient, steamClient);
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

        public class GetLeaderboardEntriesAsyncMethod : SteamClientApiClientTests
        {
            private const uint AppId = 247080;
            private const int LeaderboardId = 739999;

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

                steamClientApiClient = new SteamClientApiClient(userName, password, telemetryClient, steamClient);
            }

            private Mock<ILeaderboardEntriesCallback> mockLeaderboardEntriesCallback;
            private Mock<IAsyncJob<ILeaderboardEntriesCallback>> mockLeaderboardEntriesResponse;
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

        public class DisposeMethod : SteamClientApiClientTests
        {
            [Fact]
            public void DisposesSteamClient()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Dispose(), Times.Once);
            }

            [Fact]
            public void DisposeMoreThanOnce_DoesNothing()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClientAdapter>();
                var steamClient = mockSteamClient.Object;
                var client = new SteamClientApiClient(userName, password, telemetryClient, steamClient);

                // Act
                client.Dispose();
                client.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Never);
            }
        }
    }
}
