using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Polly;
using Polly.Timeout;
using SteamKit2;
using static SteamKit2.SteamClient;
using static SteamKit2.SteamUser;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    internal sealed class SteamClientAdapter : ISteamClientAdapter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientAdapter));

        private static readonly TimeSpan RunWaitCallbacksTimeout = TimeSpan.FromSeconds(1);
        // Fallback timeouts are normally set equal to the timeout used for the request.
        // This can cause a race condition where both timeouts expire at the same time but the
        // fallback timeout "wins". This could make it appear that SteamKit is running into a 
        // deadlock issue even though it would've timed out on its own. Adding padding to the 
        // fallback timeout's duration is an attempt at avoiding the scenario but doesn't 
        // guarantee it.
        public static readonly TimeSpan FallbackTimeoutPadding = RunWaitCallbacksTimeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientAdapter"/> class.
        /// </summary>
        /// <param name="steamClient">
        /// The Steam client.
        /// </param>
        /// <param name="manager">
        /// The callback manager associated with <paramref name="steamClient"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="steamClient"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="manager"/> is null.
        /// </exception>
        public SteamClientAdapter(ISteamClient steamClient, ICallbackManager manager) : this(steamClient, manager, null) { }

        internal SteamClientAdapter(ISteamClient steamClient, ICallbackManager manager, ILog log)
        {
            this.steamClient = steamClient ?? throw new ArgumentNullException(nameof(steamClient));
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
            this.log = log ?? Log;

            // TODO: Consider delaying creation of MessageLoop until Connect is called.
            MessageLoop = new Thread(RunMessageLoop)
            {
                IsBackground = true,
                Name = "Steam Client message loop",
            };
        }

        private readonly ISteamClient steamClient;
        private readonly ICallbackManager manager;
        private readonly ILog log;

        #region Message loop

        private bool isRunning;

        internal Thread MessageLoop { get; }

        private void StartMessageLoop()
        {
            isRunning = true;
            MessageLoop.Start();
        }

        private void RunMessageLoop()
        {
            while (isRunning)
            {
                manager.RunWaitCallbacks(RunWaitCallbacksTimeout);
            }
        }

        private void StopMessageLoop()
        {
            isRunning = false;

            if (MessageLoop.IsAlive)
            {
                MessageLoop.Join((int)(3 * RunWaitCallbacksTimeout.TotalMilliseconds));
            }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is logged on to the remote CM server.
        /// </summary>
        public bool IsLoggedOn => steamClient.SessionID != null;

        /// <summary>
        /// Gets or sets the network listening interface.
        /// </summary>
        public ProgressDebugNetworkListener ProgressDebugNetworkListener
        {
            get => steamClient.DebugNetworkListener as ProgressDebugNetworkListener;
            set => steamClient.DebugNetworkListener = value;
        }

        #region Connect

        private readonly SemaphoreSlim connectSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Connects this client to a Steam3 server. This begins the process of connecting
        /// and encrypting the data channel between the client and the server. Results are
        /// returned asynchronously in a <see cref="IConnectedCallback"/>. If the
        /// server that SteamKit attempts to connect to is down, a <see cref="IDisconnectedCallback"/>
        /// will be posted instead. SteamKit will not attempt to reconnect to Steam, you
        /// must handle this callback and call Connect again preferrably after a short delay.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task<IConnectedCallback> ConnectAsync(CancellationToken cancellationToken)
        {
            await connectSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var connectionTimeout = steamClient.ConnectionTimeout + FallbackTimeoutPadding;
                var connectionTimeoutPolicy = Policy.TimeoutAsync(connectionTimeout, TimeoutStrategy.Pessimistic, OnTimeoutAsync);

                return await ConnectAsync(connectionTimeoutPolicy, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                connectSemaphore.Release();
            }

            // TODO: Consider logging a warning if this delegate runs. It should not run unless SteamKit is 
            //       failing to honor its timeout.
            Task OnTimeoutAsync(Context context, TimeSpan duration, Task task)
            {
                StopMessageLoop();

                return Task.CompletedTask;
            }
        }

        internal Task<IConnectedCallback> ConnectAsync(
            Policy policy,
            CancellationToken cancellationToken)
        {
            return policy.ExecuteAsync(ConnectAsyncImpl, cancellationToken, continueOnCapturedContext: false);
        }

        private Task<IConnectedCallback> ConnectAsyncImpl(CancellationToken cancellationToken)
        {
            // TODO: Consider logging a warning if Try* methods fails.
            var tcs = new TaskCompletionSource<IConnectedCallback>();

            IDisposable onConnected = null;
            IDisposable onDisconnected = null;
            CancellationTokenRegistration onCancelled;
            onConnected = manager.Subscribe<IConnectedCallback>(connected =>
            {
                DisposeCallbacks();

                IDisposable onDisconnectedWhenConnected = null;
                onDisconnectedWhenConnected = manager.Subscribe<IDisconnectedCallback>(disconnected =>
                {
                    onDisconnectedWhenConnected.Dispose();

                    StopMessageLoop();
                    log.Info("Disconnected from Steam.");
                });

                log.Info("Connected to Steam.");
                tcs.TrySetResult(connected);
            });
            onDisconnected = manager.Subscribe<IDisconnectedCallback>(disconnected =>
            {
                DisposeCallbacks();

                StopMessageLoop();

                var ex = new SteamClientApiException("Unable to connect to Steam.");
                tcs.TrySetException(ex);
            });

            StartMessageLoop();

            onCancelled = cancellationToken.Register(() =>
            {
                DisposeCallbacks();

                Disconnect();
                StopMessageLoop();

                tcs.TrySetCanceled();
            });

            steamClient.Connect();

            return tcs.Task;

            void DisposeCallbacks()
            {
                onConnected.Dispose();
                onDisconnected.Dispose();
                onCancelled.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// Logs the client into the Steam3 network. The client should already have been
        /// connected at this point. Results are returned in a <see cref="ILoggedOnCallback"/>.
        /// </summary>
        /// <param name="details">The details to use for logging on.</param>
        /// <exception cref="ArgumentNullException">
        /// No logon details were provided.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Username or password are not set within details.
        /// </exception>
        public Task<ILoggedOnCallback> LogOnAsync(LogOnDetails details)
        {
            var tcs = new TaskCompletionSource<ILoggedOnCallback>();

            IDisposable onLoggedOn = null;
            IDisposable onDisconnected = null;
            onLoggedOn = manager.Subscribe<ILoggedOnCallback>(response =>
            {
                DisposeCallbacks();

                switch (response.Result)
                {
                    case EResult.OK:
                        {
                            log.Info("Logged on to Steam.");
                            tcs.SetResult(response);
                            break;
                        }
                    default:
                        {
                            var ex = new SteamClientApiException("Unable to log on to Steam.", response.Result);
                            tcs.SetException(ex);
                            break;
                        }
                }
            });
            onDisconnected = manager.Subscribe<IDisconnectedCallback>(response =>
            {
                DisposeCallbacks();

                var ex = new SteamClientApiException("Unable to log on to Steam.");
                tcs.SetException(ex);
            });

            steamClient.GetHandler<ISteamUser>().LogOn(details);

            return tcs.Task;

            void DisposeCallbacks()
            {
                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            }
        }

        /// <summary>
        /// Returns a registered handler for <see cref="SteamUserStats"/>.
        /// </summary>
        /// <returns>A registered handler on success, or null if the handler could not be found.</returns>
        public ISteamUserStats GetSteamUserStats() => steamClient.GetHandler<SteamUserStats>();

        /// <summary>
        /// Gets a value indicating whether this instance is connected to the remote CM server.
        /// </summary>
        public bool IsConnected => steamClient.IsConnected;
        /// <summary>
        /// The remote IP of the connection.
        /// </summary>
        public IPAddress RemoteIP => steamClient.RemoteIP;

        /// <summary>
        /// Disconnects this client.
        /// </summary>
        public void Disconnect() => steamClient.Disconnect();

        #region IDisposable Implementation

        private bool disposed;

        /// <summary>
        /// Disposes of resources used by <see cref="SteamClientAdapter"/>.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }

            if (IsConnected)
            {
                Disconnect();
            }
            else
            {
                StopMessageLoop();
            }

            disposed = true;
        }

        #endregion
    }
}
