using System;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientAdapterTests
    {
        public SteamClientAdapterTests()
        {
            steamClientAdapter = new SteamClientAdapter(mockSteamClient.Object, mockManager.Object);
        }

        private readonly Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
        private readonly Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
        private readonly SteamClientAdapter steamClientAdapter;

        public class Constructor
        {
            private ISteamClient steamClient = Mock.Of<ISteamClient>();
            private ICallbackManager manager = Mock.Of<ICallbackManager>();

            [Fact]
            public void SteamClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                steamClient = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientAdapter(steamClient, manager);
                });
            }

            [Fact]
            public void ManagerIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                manager = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientAdapter(steamClient, manager);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var client = new SteamClientAdapter(steamClient, manager);

                // Assert
                Assert.IsAssignableFrom<SteamClientAdapter>(client);
            }
        }

        public class IsLoggedOnProperty : SteamClientAdapterTests
        {
            [Fact]
            public void SessionIDIsNull_ReturnsFalse()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.SessionID).Returns((int?)null);

                // Act
                var isLoggedOn = steamClientAdapter.IsLoggedOn;

                // Assert
                Assert.False(isLoggedOn);
            }

            [Fact]
            public void SessionIDIsNotNull_ReturnsTrue()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.SessionID).Returns(1);

                // Act
                var isLoggedOn = steamClientAdapter.IsLoggedOn;

                // Assert
                Assert.True(isLoggedOn);
            }
        }

        public class ProgressDebugNetworkListenerProperty : SteamClientAdapterTests
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                mockSteamClient.SetupProperty(s => s.DebugNetworkListener);
                var listener = new ProgressDebugNetworkListener();

                // Act
                steamClientAdapter.ProgressDebugNetworkListener = listener;
                var listener2 = steamClientAdapter.ProgressDebugNetworkListener;

                // Assert
                Assert.Same(listener, listener2);
            }
        }

        public class GetSteamUserStatsMethod : SteamClientAdapterTests
        {
            public GetSteamUserStatsMethod()
            {
                var steamClient = new SteamClient();
                steamClientAdapter = new SteamClientAdapter(steamClient, mockManager.Object);
            }

            private readonly new SteamClientAdapter steamClientAdapter;

            [Fact]
            public void ReturnsSteamUserStats()
            {
                // Arrange -> Act
                var steamUserStats = steamClientAdapter.GetSteamUserStats();

                // Assert
                Assert.IsAssignableFrom<ISteamUserStats>(steamUserStats);
            }
        }

        public class IsConnectedProperty : SteamClientAdapterTests
        {
            [Fact]
            public void IsNotConnected_ReturnsFalse()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(false);

                // Act
                var isConnected = steamClientAdapter.IsConnected;

                // Assert
                Assert.False(isConnected);
            }

            [Fact]
            public void IsConnected_ReturnsTrue()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(true);

                // Act
                var isConnected = steamClientAdapter.IsConnected;

                // Assert
                Assert.True(isConnected);
            }
        }

        public class DisconnectMethod : SteamClientAdapterTests
        {
            [Fact]
            public void DisconectsFromSteam()
            {
                // Arrange -> Act
                steamClientAdapter.Disconnect();

                // Assert
                mockSteamClient.Verify(c => c.Disconnect(), Times.Once);
            }
        }
    }
}
