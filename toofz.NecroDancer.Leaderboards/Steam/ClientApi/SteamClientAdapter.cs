﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SteamKit2;
using static SteamKit2.SteamClient;
using static SteamKit2.SteamUser;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    internal sealed class SteamClientAdapter : ISteamClientAdapter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientAdapter));

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

        private void RunMessageLoop()
        {
            while (isRunning)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
        }

        private void StartMessageLoop()
        {
            isRunning = true;
            MessageLoop.Start();
        }

        // TODO: Does this need a join?
        private void StopMessageLoop()
        {
            isRunning = false;
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

        /// <summary>
        /// Connects this client to a Steam3 server. This begins the process of connecting
        /// and encrypting the data channel between the client and the server. Results are
        /// returned asynchronously in a <see cref="ConnectedCallback"/>. If the
        /// server that SteamKit attempts to connect to is down, a <see cref="DisconnectedCallback"/>
        /// will be posted instead. SteamKit will not attempt to reconnect to Steam, you
        /// must handle this callback and call Connect again preferrably after a short delay.
        /// </summary>
        public Task<ConnectedCallback> ConnectAsync()
        {
            var tcs = new TaskCompletionSource<ConnectedCallback>();

            IDisposable onConnected = null;
            IDisposable onDisconnected = null;
            onConnected = manager.Subscribe<ConnectedCallback>(response =>
            {
                log.Info("Connected to Steam.");
                tcs.SetResult(response);

                onConnected.Dispose();
                onDisconnected.Dispose();

                IDisposable onDisconnectedWhenConnected = null;
                onDisconnectedWhenConnected = manager.Subscribe<DisconnectedCallback>(_ =>
                {
                    StopMessageLoop();
                    log.Info("Disconnected from Steam.");

                    onDisconnectedWhenConnected.Dispose();
                });
            });
            onDisconnected = manager.Subscribe<DisconnectedCallback>(response =>
            {
                var ex = new SteamClientApiException("Unable to connect to Steam.");
                tcs.SetException(ex);

                onConnected.Dispose();
                onDisconnected.Dispose();
            });

            StartMessageLoop();
            steamClient.Connect();

            return tcs.Task;
        }

        /// <summary>
        /// Logs the client into the Steam3 network. The client should already have been
        /// connected at this point. Results are returned in a <see cref="LoggedOnCallback"/>.
        /// </summary>
        /// <param name="details">The details to use for logging on.</param>
        /// <exception cref="ArgumentNullException">
        /// No logon details were provided.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Username or password are not set within details.
        /// </exception>
        public Task<LoggedOnCallback> LogOnAsync(LogOnDetails details)
        {
            var tcs = new TaskCompletionSource<LoggedOnCallback>();

            IDisposable onLoggedOn = null;
            IDisposable onDisconnected = null;
            onLoggedOn = manager.Subscribe<LoggedOnCallback>(response =>
            {
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

                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });
            onDisconnected = manager.Subscribe<DisconnectedCallback>(response =>
            {
                var ex = new SteamClientApiException("Unable to log on to Steam.");
                tcs.SetException(ex);

                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });

            steamClient.GetHandler<SteamUser>().LogOn(details);

            return tcs.Task;
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
