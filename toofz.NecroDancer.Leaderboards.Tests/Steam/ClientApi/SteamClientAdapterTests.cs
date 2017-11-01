using System;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientAdapterTests
    {
        public class Constructor
        {
            [Fact]
            public void SteamClientIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ISteamClient steamClient = null;
                var manager = Mock.Of<ICallbackManager>();

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
                var steamClient = Mock.Of<ISteamClient>();
                ICallbackManager manager = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new SteamClientAdapter(steamClient, manager);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var steamClient = Mock.Of<ISteamClient>();
                var manager = Mock.Of<ICallbackManager>();

                // Act
                var client = new SteamClientAdapter(steamClient, manager);

                // Assert
                Assert.IsAssignableFrom<SteamClientAdapter>(client);
            }
        }


        public class IsLoggedOnProperty
        {
            public IsLoggedOnProperty()
            {
                mockSteamClient = new Mock<ISteamClient>();
                var steamClient = mockSteamClient.Object;
                var manager = Mock.Of<ICallbackManager>();
                client = new SteamClientAdapter(steamClient, manager);
            }

            Mock<ISteamClient> mockSteamClient;
            SteamClientAdapter client;

            [Fact]
            public void SessionIDIsNull_ReturnsFalse()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.SessionID).Returns((int?)null);

                // Act
                var isLoggedOn = client.IsLoggedOn;

                // Assert
                Assert.False(isLoggedOn);
            }

            [Fact]
            public void SessionIDIsNotNull_ReturnsTrue()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.SessionID).Returns(1);

                // Act
                var isLoggedOn = client.IsLoggedOn;

                // Assert
                Assert.True(isLoggedOn);
            }
        }


        public class ProgressDebugNetworkListenerProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var steamClient = Mock.Of<ISteamClient>();
                var manager = Mock.Of<ICallbackManager>();
                var client = new SteamClientAdapter(steamClient, manager);
                var listener = new ProgressDebugNetworkListener();

                // Act
                client.ProgressDebugNetworkListener = listener;
                var listener2 = client.ProgressDebugNetworkListener;

                // Assert
                Assert.Same(listener, listener2);
            }
        }


        public class GetSteamUserStatsMethod
        {
            [Fact]
            public void ReturnsSteamUserStats()
            {
                // Arrange
                var steamClient = new SteamClient();
                var manager = Mock.Of<ICallbackManager>();
                var client = new SteamClientAdapter(steamClient, manager);

                // Act
                var steamUserStats = client.GetSteamUserStats();

                // Assert
                Assert.IsAssignableFrom<ISteamUserStats>(steamUserStats);
            }
        }


        public class IsConnectedProperty
        {
            public IsConnectedProperty()
            {
                mockSteamClient = new Mock<ISteamClient>();
                var steamClient = mockSteamClient.Object;
                var manager = Mock.Of<ICallbackManager>();
                client = new SteamClientAdapter(steamClient, manager);
            }

            Mock<ISteamClient> mockSteamClient;
            SteamClientAdapter client;

            [Fact]
            public void IsNotConnected_ReturnsFalse()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(false);

                // Act
                var isConnected = client.IsConnected;

                // Assert
                Assert.False(isConnected);
            }

            [Fact]
            public void IsConnected_ReturnsTrue()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(true);

                // Act
                var isConnected = client.IsConnected;

                // Assert
                Assert.True(isConnected);
            }
        }


        public class DisconnectMethod
        {
            [Fact]
            public void DisconectsFromSteam()
            {
                // Arrange
                var mockSteamClient = new Mock<ISteamClient>();
                var steamClient = mockSteamClient.Object;
                var manager = Mock.Of<ICallbackManager>();
                var client = new SteamClientAdapter(steamClient, manager);

                // Act
                client.Disconnect();

                // Assert
                mockSteamClient.Verify(c => c.Disconnect(), Times.Once);
            }
        }
    }
}
