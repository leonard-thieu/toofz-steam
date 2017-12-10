using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Moq;
using Polly;
using Polly.Timeout;
using SteamKit2;
using toofz.Steam.ClientApi;
using Xunit;
using static SteamKit2.SteamUser;
using static SteamKit2.SteamUserStats;

namespace toofz.Steam.Tests.ClientApi
{
    public class SteamClientApiClientTests
    {
        public SteamClientApiClientTests()
        {
            steamClientApiClient = new SteamClientApiClient(userName, password, retryPolicy, telemetryClient, mockSteamClient.Object);
        }

        private readonly string userName = "myUserName";
        private readonly string password = "myPassword";
        private readonly Policy retryPolicy = Policy.NoOpAsync();
        private readonly Mock<ISteamClientAdapter> mockSteamClient = new Mock<ISteamClientAdapter>();
        private readonly TelemetryClient telemetryClient = new TelemetryClient();
        private readonly SteamClientApiClient steamClientApiClient;

        public class IsTransientMethod
        {
            [DisplayFact(nameof(SteamClientApiException), nameof(SteamClientApiException.Result))]
            public void ExIsSteamClientApiExceptionAndResultIsNull_ReturnsTrue()
            {
                // Arrange
                var ex = new SteamClientApiException(null, new TaskCanceledException());

                // Act
                var isTransient = SteamClientApiClient.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [DisplayFact(nameof(SteamClientApiException), nameof(SteamClientApiException.Result))]
            public void ExIsSteamClientApiExceptionAndResultIsNotNull_ReturnsFalse()
            {
                // Arrange
                var ex = new SteamClientApiException(null, EResult.AccessDenied);

                // Act
                var isTransient = SteamClientApiClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }

            [DisplayFact(nameof(TimeoutRejectedException))]
            public void ExIsTimeoutRejectedException_ReturnsTrue()
            {
                // Arrange
                var ex = new TimeoutRejectedException();

                // Act
                var isTransient = SteamClientApiClient.IsTransient(ex);

                // Assert
                Assert.True(isTransient);
            }

            [DisplayFact]
            public void ReturnsFalse()
            {
                // Arrange
                var ex = new TaskCanceledException();

                // Act
                var isTransient = SteamClientApiClient.IsTransient(ex);

                // Assert
                Assert.False(isTransient);
            }
        }

        public class Constructor
        {
            private string userName = "myUserName";
            private string password = "myPassword";
            private Policy retryPolicy = Policy.NoOpAsync();
            private TelemetryClient telemetryClient = new TelemetryClient();

            [DisplayFact(nameof(ArgumentNullException))]
            public void UserNameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                userName = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);
                });
            }

            [DisplayFact(nameof(ArgumentException))]
            public void UserNameIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                userName = "";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);
                });
            }

            [DisplayFact(nameof(ArgumentNullException))]
            public void PasswordIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                password = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);
                });
            }

            [DisplayFact(nameof(ArgumentException))]
            public void PasswordIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                password = "";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);
                });
            }

            [DisplayFact(nameof(TelemetryClient), nameof(ArgumentNullException))]
            public void TelemetryClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                telemetryClient = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);
                });
            }

            [DisplayFact(nameof(ArgumentNullException))]
            public void PolicyIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                retryPolicy = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);
                });
            }

            [DisplayFact(nameof(SteamClientApiClient))]
            public void ReturnsSteamClientApiClient()
            {
                // Arrange -> Act
                var steamClientApiClient = new SteamClientApiClient(userName, password, retryPolicy, telemetryClient);

                // Assert
                Assert.IsAssignableFrom<SteamClientApiClient>(steamClientApiClient);
            }
        }

        public class ProgressGetter : SteamClientApiClientTests
        {
            [DisplayFact(nameof(ObjectDisposedException))]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                steamClientApiClient.Dispose();

                // Act -> Assert
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    steamClientApiClient.Progress.ToString();
                });
            }

            [DisplayFact(nameof(SteamClientApiClient.Progress))]
            public void GetsProgress()
            {
                // Arrange
                var listener = new ProgressDebugNetworkListener { Progress = Mock.Of<IProgress<long>>() };
                mockSteamClient.SetupGet(c => c.ProgressDebugNetworkListener).Returns(listener);

                // Act
                var progress = steamClientApiClient.Progress;

                // Assert
                Assert.IsAssignableFrom<IProgress<long>>(progress);
            }
        }

        public class ProgressSetter : SteamClientApiClientTests
        {
            [DisplayFact(nameof(ObjectDisposedException))]
            public void IsDisposed_ThrowsObjectDisposedException()
            {
                // Arrange
                steamClientApiClient.Dispose();

                // Act -> Assert
                Assert.Throws<ObjectDisposedException>(() =>
                {
                    steamClientApiClient.Progress = null;
                });
            }

            [DisplayFact(nameof(SteamClientApiClient.Progress))]
            public void SetsProgress()
            {
                // Arrange
                var listener = new ProgressDebugNetworkListener();
                mockSteamClient.SetupGet(c => c.ProgressDebugNetworkListener).Returns(listener);
                var progress = Mock.Of<IProgress<long>>();

                // Act
                steamClientApiClient.Progress = progress;

                // Assert
                Assert.IsAssignableFrom<IProgress<long>>(steamClientApiClient.Progress);
            }
        }

        public class TimeoutProperty : SteamClientApiClientTests
        {
            [DisplayFact]
            public void NotSet_ReturnsDefault()
            {
                // Arrange -> Act
                var timeout = steamClientApiClient.Timeout;

                // Assert
                Assert.Equal(TimeSpan.FromSeconds(10), timeout);
            }

            [DisplayFact(nameof(SteamClientApiClient.Timeout))]
            public void GetsAndSetsTimeout()
            {
                // Arrange
                var timeout = TimeSpan.FromSeconds(1);

                // Act
                steamClientApiClient.Timeout = timeout;
                var timeout2 = steamClientApiClient.Timeout;

                // Assert
                Assert.Equal(timeout, timeout2);
            }
        }

        public class ConnectAndLogOnAsyncMethod : SteamClientApiClientTests
        {
            private readonly CancellationToken cancellationToken = CancellationToken.None;

            [DisplayFact]
            public async Task NotConnected_Connects()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act
                await steamClientApiClient.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(cancellationToken), Times.Once);
            }

            [DisplayFact]
            public async Task Connected_DoesNotConnect()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);

                // Act
                await steamClientApiClient.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.ConnectAsync(cancellationToken), Times.Never);
            }

            [DisplayFact]
            public async Task NotLoggedOn_LogsOn()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act
                await steamClientApiClient.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.LogOnAsync(It.IsAny<LogOnDetails>()), Times.Once);
            }

            [DisplayFact]
            public async Task LoggedOn_DoesNotLogOn()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);

                // Act
                await steamClientApiClient.ConnectAndLogOnAsync();

                // Assert
                mockSteamClient.Verify(c => c.LogOnAsync(It.IsAny<LogOnDetails>()), Times.Never);
            }
        }

        public class DisconnectMethod : SteamClientApiClientTests
        {
            [DisplayFact("Steam")]
            public void DisconnectsFromSteam()
            {
                // Arrange -> Act
                steamClientApiClient.Disconnect();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Once);
            }
        }

        public class FindLeaderboardAsyncMethod : SteamClientApiClientTests
        {
            public FindLeaderboardAsyncMethod()
            {
                mockLeaderboardResponse.Setup(r => r.ToTask()).ReturnsAsync(mockLeaderboardCallback.Object);

                mockSteamUserStats
                    .Setup(s => s.FindLeaderboard(It.IsAny<uint>(), It.IsAny<string>()))
                    .Returns(mockLeaderboardResponse.Object);
                mockSteamClient.Setup(c => c.GetSteamUserStats()).Returns(mockSteamUserStats.Object);

                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
            }

            private readonly Mock<IFindOrCreateLeaderboardCallback> mockLeaderboardCallback = new Mock<IFindOrCreateLeaderboardCallback>();
            private readonly Mock<IAsyncJob<IFindOrCreateLeaderboardCallback>> mockLeaderboardResponse = new Mock<IAsyncJob<IFindOrCreateLeaderboardCallback>>();
            private readonly Mock<ISteamUserStats> mockSteamUserStats = new Mock<ISteamUserStats>();
            private readonly uint appId = 247080;
            private readonly string leaderboardName = "Leaderboard Name";

            [DisplayFact(nameof(InvalidOperationException))]
            public async Task NotConnected_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(appId, leaderboardName);
                });
            }

            [DisplayFact(nameof(InvalidOperationException))]
            public async Task NotLoggedOn_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(appId, leaderboardName);
                });
            }

            [DisplayFact(nameof(EResult.OK), nameof(SteamClientApiException))]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.Fail);

                // Act -> Assert
                var ex = await Assert.ThrowsAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(appId, leaderboardName);
                });
                Assert.Equal(EResult.Fail, ex.Result);
            }

            [DisplayFact(nameof(SteamClientApiException))]
            public async Task IDIsZero_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);
                mockLeaderboardCallback.Setup(le => le.ID).Returns(0);

                // Act -> Assert
                await Assert.ThrowsAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.FindLeaderboardAsync(appId, leaderboardName);
                });
            }

            [DisplayFact(nameof(EResult.OK))]
            public async Task ResultIsOK_ReturnsLeaderboard()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);
                mockLeaderboardCallback.Setup(le => le.ID).Returns(1);

                // Act
                var leaderboard = await steamClientApiClient.FindLeaderboardAsync(appId, leaderboardName);

                // Assert
                Assert.IsAssignableFrom<IFindOrCreateLeaderboardCallback>(leaderboard);
            }

            [DisplayFact]
            public async Task SetsTimeout()
            {
                // Arrange
                mockLeaderboardCallback.Setup(le => le.Result).Returns(EResult.OK);
                mockLeaderboardCallback.Setup(le => le.ID).Returns(1);

                // Act
                await steamClientApiClient.FindLeaderboardAsync(appId, leaderboardName);

                // Assert
                mockLeaderboardResponse.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
            }
        }

        public class GetLeaderboardEntriesAsyncMethod : SteamClientApiClientTests
        {
            public GetLeaderboardEntriesAsyncMethod()
            {
                mockLeaderboardEntriesResponse.Setup(r => r.ToTask()).ReturnsAsync(mockLeaderboardEntriesCallback.Object);

                mockSteamUserStats
                    .Setup(s => s.GetLeaderboardEntries(It.IsAny<uint>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ELeaderboardDataRequest>()))
                    .Returns(mockLeaderboardEntriesResponse.Object);
                mockSteamClient.Setup(c => c.GetSteamUserStats()).Returns(mockSteamUserStats.Object);

                mockSteamClient.SetupGet(c => c.IsConnected).Returns(true);
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(true);
            }

            private readonly Mock<ILeaderboardEntriesCallback> mockLeaderboardEntriesCallback = new Mock<ILeaderboardEntriesCallback>();
            private readonly Mock<IAsyncJob<ILeaderboardEntriesCallback>> mockLeaderboardEntriesResponse = new Mock<IAsyncJob<ILeaderboardEntriesCallback>>();
            private readonly Mock<ISteamUserStats> mockSteamUserStats = new Mock<ISteamUserStats>();
            private readonly uint appId = 247080;
            private readonly int leaderboardId = 739999;

            [DisplayFact(nameof(InvalidOperationException))]
            public async Task NotConnected_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsConnected).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(appId, leaderboardId);
                });
            }

            [DisplayFact(nameof(InvalidOperationException))]
            public async Task NotLoggedOn_ThrowsInvalidOperationException()
            {
                // Arrange
                mockSteamClient.SetupGet(c => c.IsLoggedOn).Returns(false);

                // Act -> Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(appId, leaderboardId);
                });
            }

            [DisplayFact(nameof(EResult.OK), nameof(SteamClientApiException))]
            public async Task ResultIsNotOK_ThrowsSteamClientApiException()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.Fail);

                // Act -> Assert
                var ex = await Assert.ThrowsAsync<SteamClientApiException>(() =>
                {
                    return steamClientApiClient.GetLeaderboardEntriesAsync(appId, leaderboardId);
                });
                Assert.Equal(EResult.Fail, ex.Result);
            }

            [DisplayFact(nameof(EResult.OK))]
            public async Task ResultIsOK_ReturnsLeaderboardEntries()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                var leaderboardEntries = await steamClientApiClient.GetLeaderboardEntriesAsync(appId, leaderboardId);

                // Assert
                Assert.IsAssignableFrom<ILeaderboardEntriesCallback>(leaderboardEntries);
            }

            [DisplayFact]
            public async Task SetsTimeout()
            {
                // Arrange
                mockLeaderboardEntriesCallback.Setup(le => le.Result).Returns(EResult.OK);

                // Act
                await steamClientApiClient.GetLeaderboardEntriesAsync(appId, leaderboardId);

                // Assert
                mockLeaderboardEntriesResponse.VerifySet(s => s.Timeout = It.IsAny<TimeSpan>(), Times.Once);
            }
        }

        public class DisposeMethod : SteamClientApiClientTests
        {
            [DisplayFact(nameof(SteamClient))]
            public void DisposesSteamClient()
            {
                // Arrange -> Act
                steamClientApiClient.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Dispose(), Times.Once);
            }

            [DisplayFact]
            public void DisposeMoreThanOnce_DoesNothing()
            {
                // Arrange -> Act
                steamClientApiClient.Dispose();
                steamClientApiClient.Dispose();

                // Assert
                mockSteamClient.Verify(s => s.Disconnect(), Times.Never);
            }
        }
    }
}
