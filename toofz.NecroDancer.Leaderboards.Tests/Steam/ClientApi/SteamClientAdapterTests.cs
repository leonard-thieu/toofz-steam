using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SteamKit2;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;
using Xunit;
using static SteamKit2.SteamClient;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    public class SteamClientAdapterTests : IDisposable
    {
        public SteamClientAdapterTests()
        {
            steamClientAdapter = new SteamClientAdapter(mockSteamClient.Object, mockManager.Object);
        }

        private readonly Mock<ISteamClient> mockSteamClient = new Mock<ISteamClient>();
        private readonly Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
        private readonly SteamClientAdapter steamClientAdapter;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            steamClientAdapter.Dispose();
        }

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

        public class ConnectAsyncMethod : SteamClientAdapterTests
        {
            public ConnectAsyncMethod()
            {
                mockManager
                    .Setup(m => m.Subscribe(It.Is<Action<ConnectedCallback>>(cb => SetOnConnected(cb))))
                    .Returns(mockUnsubscribeOnConnected.Object);
                mockManager
                    .SetupSequence(m => m.Subscribe(It.Is<Action<DisconnectedCallback>>(cb => SetOnDisconnected(cb))))
                    .Returns(mockUnsubscribeOnDisconnected.Object)
                    .Returns(mockUnsubscribeOnDisconnectedWhenConnected.Object);

                responseTask = steamClientAdapter.ConnectAsync();
                onConnected(null);
            }

            private readonly Mock<IDisposable> mockUnsubscribeOnConnected = new Mock<IDisposable>();
            private readonly Mock<IDisposable> mockUnsubscribeOnDisconnected = new Mock<IDisposable>();
            private readonly Mock<IDisposable> mockUnsubscribeOnDisconnectedWhenConnected = new Mock<IDisposable>();

            private readonly Task<ConnectedCallback> responseTask;

            private Action<ConnectedCallback> onConnected;
            private Action<DisconnectedCallback> onDisconnected;
            private Action<DisconnectedCallback> onDisconnectedWhenConnected;

            private bool SetOnConnected(Action<ConnectedCallback> cb)
            {
                onConnected = cb;

                return true;
            }

            private bool SetOnDisconnected(Action<DisconnectedCallback> cb)
            {
                if (onDisconnected == null)
                {
                    onDisconnected = cb;
                }
                else
                {
                    onDisconnectedWhenConnected = cb;
                }

                return true;
            }

            public class Connects : ConnectAsyncMethod
            {
                [Fact]
                public async Task ReturnsResponse()
                {
                    // Arrange -> Act
                    var response = await responseTask;

                    // Assert
                    Assert.Null(response);
                }

                [Fact]
                public async Task DisposesOnConnected()
                {
                    // Arrange -> Act
                    var response = await responseTask;

                    // Assert
                    mockUnsubscribeOnConnected.Verify(d => d.Dispose(), Times.Once);
                }

                [Fact]
                public async Task DisposesOnDisconnected()
                {
                    // Arrange -> Act
                    var response = await responseTask;

                    // Assert
                    mockUnsubscribeOnDisconnected.Verify(d => d.Dispose(), Times.Once);
                }

                [Fact]
                public async Task SetsOnDisconnectedWhenConnected()
                {
                    // Arrange -> Act
                    var response = await responseTask;

                    // Assert
                    Assert.NotNull(onDisconnectedWhenConnected);
                }

                public class ThenDisconnects : Connects
                {
                    public ThenDisconnects()
                    {
                        onDisconnectedWhenConnected(null);
                    }

                    [Fact]
                    public async Task StopsMessageLoop()
                    {
                        // Arrange -> Act
                        await responseTask;

                        // Assert
                        Assert.Equal(ThreadState.Stopped, steamClientAdapter.MessageLoop.ThreadState);
                    }

                    [Fact]
                    public async Task DisposesOnDisconnectedWhenConnected()
                    {
                        // Arrange -> Act
                        await responseTask;

                        // Assert
                        mockUnsubscribeOnDisconnectedWhenConnected.Verify(d => d.Dispose(), Times.Once);
                    }
                }
            }
        }

        public class GetSteamUserStatsMethod
        {
            public GetSteamUserStatsMethod()
            {
                steamClientAdapter = new SteamClientAdapter(steamClient, mockManager.Object);
            }

            private readonly SteamClient steamClient = new SteamClient();
            private readonly Mock<ICallbackManager> mockManager = new Mock<ICallbackManager>();
            private readonly SteamClientAdapter steamClientAdapter;

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
