using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polly;
using SteamKit2;
using toofz.Steam.ClientApi;
using Xunit;
using static SteamKit2.SteamClient;
using static SteamKit2.SteamUser;

namespace toofz.Steam.Tests.ClientApi
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

            [DisplayFact(nameof(SteamClient), nameof(ArgumentNullException))]
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

            [DisplayFact(nameof(ArgumentNullException))]
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

            [DisplayFact(nameof(SteamClientAdapter))]
            public void ReturnsSteamClientAdapter()
            {
                // Arrange -> Act
                var client = new SteamClientAdapter(steamClient, manager);

                // Assert
                Assert.IsAssignableFrom<SteamClientAdapter>(client);
            }
        }

        public class IsLoggedOnProperty : SteamClientAdapterTests
        {
            [DisplayFact(nameof(SteamClient.SessionID))]
            public void SessionIDIsNull_ReturnsFalse()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.SessionID).Returns((int?)null);

                // Act
                var isLoggedOn = steamClientAdapter.IsLoggedOn;

                // Assert
                Assert.False(isLoggedOn);
            }

            [DisplayFact(nameof(SteamClient.SessionID))]
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
            [DisplayFact(nameof(SteamClientAdapter.ProgressDebugNetworkListener))]
            public void GetsAndSetsProgressDebugNetworkListener()
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
                    .Setup(m => m.Subscribe(It.Is<Action<IConnectedCallback>>(cb => SetOnConnected(cb))))
                    .Returns(mockUnsubscribeOnConnected.Object);

                mockManager
                    .SetupSequence(m => m.Subscribe(It.Is<Action<IDisconnectedCallback>>(cb => SetOnDisconnected(cb))))
                    .Returns(mockUnsubscribeOnDisconnected.Object)
                    .Returns(mockUnsubscribeOnDisconnectedWhenConnected.Object);
            }

            private readonly Mock<IDisposable> mockUnsubscribeOnConnected = new Mock<IDisposable>();
            private readonly Mock<IDisposable> mockUnsubscribeOnDisconnected = new Mock<IDisposable>();
            private readonly Mock<IDisposable> mockUnsubscribeOnDisconnectedWhenConnected = new Mock<IDisposable>();
            private readonly Policy connectPolicy = Policy.NoOpAsync();
            private CancellationToken cancellationToken;

            private Action<IConnectedCallback> onConnected;
            private Action<IDisconnectedCallback> onDisconnected;
            private Action<IDisconnectedCallback> onDisconnectedWhenConnected;

            private bool SetOnConnected(Action<IConnectedCallback> cb)
            {
                onConnected = cb;

                return true;
            }

            private bool SetOnDisconnected(Action<IDisconnectedCallback> callback)
            {
                if (onDisconnected == null)
                {
                    onDisconnected = callback;
                }
                else if (callback != onDisconnected &&
                         onDisconnectedWhenConnected == null)
                {
                    onDisconnectedWhenConnected = callback;
                }

                return true;
            }

            [DisplayFact]
            public async Task FallbackTimeoutExpires_CanReconnect()
            {
                // Arrange
                var retryCount = 0;
                var policy = Policy
                    .Handle<Exception>(SteamClientApiClient.IsTransient)
                    .WaitAndRetryAsync(
                        new[] { TimeSpan.FromTicks(1) },
                        (e, d, r, c) =>
                        {
                            retryCount = r;
                        });

                Func<CancellationToken, Task<IConnectedCallback>> connect = ct =>
                {
                    var connectTask = steamClientAdapter.ConnectAsync(ct);
                    if (retryCount == 1)
                    {
                        onConnected(Mock.Of<IConnectedCallback>());
                    }

                    return connectTask;
                };

                // Act
                await policy.ExecuteAsync(connect, cancellationToken, continueOnCapturedContext: false);

                // Assert
                Assert.Equal(1, retryCount);
                Assert.NotNull(steamClientAdapter.MessageLoop);
            }

            public class OnConnectedTests : ConnectAsyncMethod
            {
                private readonly Mock<IConnectedCallback> mockConnectedCallback = new Mock<IConnectedCallback>();

                [DisplayFact("OnConnected")]
                public async Task DisposesOnConnected()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.ConnectAsync(cancellationToken);
                    onConnected(mockConnectedCallback.Object);

                    // Act
                    var response = await responseTask;

                    // Assert
                    mockUnsubscribeOnConnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact("OnDisconnected")]
                public async Task DisposesOnDisconnected()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.ConnectAsync(cancellationToken);
                    onConnected(mockConnectedCallback.Object);

                    // Act
                    var response = await responseTask;

                    // Assert
                    mockUnsubscribeOnDisconnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact]
                public async Task ReturnsResponse()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.ConnectAsync(cancellationToken);
                    onConnected(mockConnectedCallback.Object);

                    // Act
                    var response = await responseTask;

                    // Assert
                    Assert.IsAssignableFrom<IConnectedCallback>(response);
                }

                [DisplayFact("OnDisconnectedWhenConnected")]
                public async Task SetsOnDisconnectedWhenConnected()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.ConnectAsync(cancellationToken);
                    onConnected(mockConnectedCallback.Object);

                    // Act
                    await responseTask;

                    // Assert
                    Assert.NotNull(onDisconnectedWhenConnected);
                }

                public class OnDisconnectedWhenConnectedTests : OnConnectedTests
                {
                    private readonly Mock<IDisconnectedCallback> mockDisconnectedCallback = new Mock<IDisconnectedCallback>();

                    [DisplayFact("OnDisconnectedWhenConnected")]
                    public async Task DisposesOnDisconnectedWhenConnected()
                    {
                        // Arrange
                        var responseTask = steamClientAdapter.ConnectAsync(cancellationToken);
                        onConnected(mockConnectedCallback.Object);
                        onDisconnectedWhenConnected(mockDisconnectedCallback.Object);

                        // Act
                        await responseTask;

                        // Assert
                        mockUnsubscribeOnDisconnectedWhenConnected.Verify(d => d.Dispose(), Times.Once);
                    }

                    [DisplayFact]
                    public async Task StopsMessageLoop()
                    {
                        // Arrange
                        var responseTask = steamClientAdapter.ConnectAsync(cancellationToken);
                        onConnected(mockConnectedCallback.Object);
                        onDisconnectedWhenConnected(mockDisconnectedCallback.Object);

                        // Act
                        await responseTask;

                        // Assert
                        Assert.Null(steamClientAdapter.MessageLoop);
                    }
                }
            }

            public class OnDisconnectedTests : ConnectAsyncMethod
            {
                private readonly Mock<IDisconnectedCallback> mockOnDisconnected = new Mock<IDisconnectedCallback>();

                [DisplayFact("OnConnected")]
                public void DisposesOnConnected()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    onDisconnected(mockOnDisconnected.Object);

                    // Assert
                    mockUnsubscribeOnConnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact("OnDisconnected")]
                public void DisposesOnDisconnected()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    onDisconnected(mockOnDisconnected.Object);

                    // Assert
                    mockUnsubscribeOnDisconnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact]
                public void StopsMessageLoop()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    onDisconnected(mockOnDisconnected.Object);

                    // Assert
                    Assert.Null(steamClientAdapter.MessageLoop);
                }

                [DisplayFact(nameof(SteamClientApiException))]
                public async Task ThrowsSteamClientApiException()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);
                    onDisconnected(mockOnDisconnected.Object);

                    // Act -> Assert
                    await Assert.ThrowsAsync<SteamClientApiException>(() =>
                    {
                        return responseTask;
                    });
                }
            }

            public class OnCancelledTests : ConnectAsyncMethod
            {
                public OnCancelledTests()
                {
                    cancellationToken = cts.Token;
                }

                private readonly CancellationTokenSource cts = new CancellationTokenSource();

                [DisplayFact("OnConnected")]
                public void DisposesOnConnected()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    cts.Cancel();

                    // Assert
                    mockUnsubscribeOnConnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact("OnDisconnected")]
                public void DisposesOnDisconnected()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    cts.Cancel();

                    // Assert
                    mockUnsubscribeOnDisconnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact]
                public void Disconnects()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    cts.Cancel();

                    // Assert
                    mockSteamClient.Verify(s => s.Disconnect(), Times.Once);
                }

                [DisplayFact]
                public void StopsMessageLoop()
                {
                    // Arrange
                    steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    cts.Cancel();

                    // Assert
                    Assert.Null(steamClientAdapter.MessageLoop);
                }

                [DisplayFact(nameof(TaskCanceledException))]
                public async Task ThrowsTaskCanceledException()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.ConnectAsync(connectPolicy, cancellationToken);

                    // Act
                    cts.Cancel();

                    // Assert
                    await Assert.ThrowsAsync<TaskCanceledException>(() =>
                    {
                        return responseTask;
                    });
                }
            }
        }

        public class LogOnAsyncMethod : SteamClientAdapterTests
        {
            public LogOnAsyncMethod()
            {
                mockSteamClient.Setup(c => c.GetHandler<ISteamUser>()).Returns(mockSteamUser.Object);

                mockManager
                    .Setup(m => m.Subscribe(It.Is<Action<ILoggedOnCallback>>(cb => SetOnLoggedOn(cb))))
                    .Returns(mockUnsubscribeOnLoggedOn.Object);

                mockManager
                    .Setup(m => m.Subscribe(It.Is<Action<IDisconnectedCallback>>(cb => SetOnDisconnected(cb))))
                    .Returns(mockUnsubscribeOnDisconnected.Object);
            }

            private readonly Mock<IDisposable> mockUnsubscribeOnLoggedOn = new Mock<IDisposable>();
            private readonly Mock<IDisposable> mockUnsubscribeOnDisconnected = new Mock<IDisposable>();
            private readonly Mock<ISteamUser> mockSteamUser = new Mock<ISteamUser>();

            private Action<ILoggedOnCallback> onLoggedOn;
            private Action<IDisconnectedCallback> onDisconnected;

            private bool SetOnLoggedOn(Action<ILoggedOnCallback> cb)
            {
                onLoggedOn = cb;

                return true;
            }

            private bool SetOnDisconnected(Action<IDisconnectedCallback> cb)
            {
                onDisconnected = cb;

                return true;
            }

            [DisplayFact(nameof(Task))]
            public void ReturnsTask()
            {
                // Arrange -> Act
                var responseTask = steamClientAdapter.LogOnAsync(new LogOnDetails());

                // Assert
                Assert.IsAssignableFrom<Task<ILoggedOnCallback>>(responseTask);
            }

            public class OnLoggedOnTests : LogOnAsyncMethod
            {
                private readonly Mock<ILoggedOnCallback> mockLoggedOnCallback = new Mock<ILoggedOnCallback>();

                [DisplayFact("OnLoggedOn")]
                public async Task DisposesOnLoggedOn()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.LogOnAsync(new LogOnDetails());
                    mockLoggedOnCallback.Setup(r => r.Result).Returns(EResult.OK);
                    onLoggedOn(mockLoggedOnCallback.Object);

                    // Act
                    await responseTask;

                    // Assert
                    mockUnsubscribeOnLoggedOn.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact("OnDisconnected")]
                public async Task DisposesOnDisconnected()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.LogOnAsync(new LogOnDetails());
                    mockLoggedOnCallback.Setup(r => r.Result).Returns(EResult.OK);
                    onLoggedOn(mockLoggedOnCallback.Object);

                    // Act
                    await responseTask;

                    // Assert
                    mockUnsubscribeOnDisconnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact(nameof(EResult.OK))]
                public async Task ResultIsOK_ReturnsResponse()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.LogOnAsync(new LogOnDetails());
                    mockLoggedOnCallback.Setup(r => r.Result).Returns(EResult.OK);
                    onLoggedOn(mockLoggedOnCallback.Object);

                    // Act
                    var response = await responseTask;

                    // Assert
                    Assert.IsAssignableFrom<ILoggedOnCallback>(response);
                }

                [DisplayFact(nameof(EResult.OK), nameof(SteamClientApiException))]
                public async Task ResultIsNotOK_ThrowsSteamClientApiException()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.LogOnAsync(new LogOnDetails());
                    mockLoggedOnCallback.Setup(r => r.Result).Returns(EResult.AccessDenied);
                    onLoggedOn(mockLoggedOnCallback.Object);

                    // Act -> Assert
                    await Assert.ThrowsAsync<SteamClientApiException>(() =>
                    {
                        return responseTask;
                    });
                }
            }

            public class OnDisconnectedTests : LogOnAsyncMethod
            {
                private readonly Mock<IDisconnectedCallback> mockOnDisconnected = new Mock<IDisconnectedCallback>();

                [DisplayFact("OnLoggedOn")]
                public void DisposesOnLoggedOn()
                {
                    // Arrange
                    steamClientAdapter.LogOnAsync(new LogOnDetails());

                    // Act
                    onDisconnected(mockOnDisconnected.Object);

                    // Assert
                    mockUnsubscribeOnLoggedOn.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact("OnDisconnected")]
                public void DisposesOnDisconnected()
                {
                    // Arrange
                    steamClientAdapter.LogOnAsync(new LogOnDetails());

                    // Act
                    onDisconnected(mockOnDisconnected.Object);

                    // Assert
                    mockUnsubscribeOnDisconnected.Verify(d => d.Dispose(), Times.Once);
                }

                [DisplayFact(nameof(SteamClientApiException))]
                public async Task ThrowsSteamClientApiException()
                {
                    // Arrange
                    var responseTask = steamClientAdapter.LogOnAsync(new LogOnDetails());
                    onDisconnected(mockOnDisconnected.Object);

                    // Act -> Assert
                    await Assert.ThrowsAsync<SteamClientApiException>(() =>
                    {
                        return responseTask;
                    });
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

            [DisplayFact(nameof(SteamUserStats))]
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
            [DisplayFact]
            public void IsNotConnected_ReturnsFalse()
            {
                // Arrange
                mockSteamClient.SetupGet(s => s.IsConnected).Returns(false);

                // Act
                var isConnected = steamClientAdapter.IsConnected;

                // Assert
                Assert.False(isConnected);
            }

            [DisplayFact]
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

        public class RemoteIPProperty : SteamClientAdapterTests
        {
            [DisplayFact(nameof(SteamClientAdapter.RemoteIP))]
            public void ReturnsRemoteIP()
            {
                // Arrange
                mockSteamClient.Setup(s => s.RemoteIP).Returns(new IPAddress(new byte[] { 127, 0, 0, 1 }));

                // Act
                var remoteIP = steamClientAdapter.RemoteIP;

                // Assert
                Assert.IsAssignableFrom<IPAddress>(remoteIP);
            }
        }

        public class DisconnectMethod : SteamClientAdapterTests
        {
            [DisplayFact("Steam")]
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
