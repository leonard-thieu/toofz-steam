using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SteamKit2;
using static SteamKit2.SteamClient;
using static SteamKit2.SteamUser;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    sealed class SteamClientAdapter : ISteamClientAdapter
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientAdapter));

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
        public SteamClientAdapter(ISteamClient steamClient, ICallbackManager manager)
        {
            this.steamClient = steamClient ?? throw new ArgumentNullException(nameof(steamClient), $"{nameof(steamClient)} is null.");
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager), $"{nameof(manager)} is null.");
            MessageLoop = new Thread(() =>
            {
                while (true)
                {
                    this.manager.RunWaitCallbacks();
                }
            });
            MessageLoop.Start();
        }

        readonly ISteamClient steamClient;
        readonly ICallbackManager manager;

        internal Thread MessageLoop { get; }

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
        /// <param name="cmServer">
        /// The <see cref="IPEndPoint"/> of the CM server to connect to. If null, SteamKit will
        /// randomly select a CM server from its internal list.
        /// </param>
        public Task<ConnectedCallback> ConnectAsync(IPEndPoint cmServer = null)
        {
            var tcs = new TaskCompletionSource<ConnectedCallback>();

            IDisposable onConnected = null;
            IDisposable onDisconnected = null;
            onConnected = manager.Subscribe<ConnectedCallback>(response =>
            {
                switch (response.Result)
                {
                    case EResult.OK:
                        {
                            Log.Info("Connected to Steam.");
                            tcs.TrySetResult(response);
                            break;
                        }
                    default:
                        {
                            tcs.TrySetException(new SteamClientApiException($"Unable to connect to Steam.", response.Result));
                            break;
                        }
                }

                onConnected.Dispose();
                onDisconnected.Dispose();

                onDisconnected = manager.Subscribe<DisconnectedCallback>(_ =>
                {
                    Log.Info("Disconnected from Steam.");
                    onDisconnected.Dispose();
                });
            });
            onDisconnected = manager.Subscribe<DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamClientApiException("Unable to connect to Steam."));
                onConnected.Dispose();
                onDisconnected.Dispose();
            });

            steamClient.Connect(cmServer);

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
                            Log.Info("Logged on to Steam.");
                            tcs.TrySetResult(response);
                            break;
                        }
                    default:
                        {
                            var ex = new SteamClientApiException("Unable to logon to Steam.", response.Result);
                            tcs.TrySetException(ex);
                            break;
                        }
                }

                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });
            onDisconnected = manager.Subscribe<DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamClientApiException("Unable to connect to Steam."));
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
        /// Returns the the local IP of this client.
        /// </summary>
        public IPAddress LocalIP => steamClient.LocalIP;
        /// <summary>
        /// Gets the connected universe of this client. This value will be <see cref="EUniverse.Invalid"/> 
        /// if the client is not connected to Steam.
        /// </summary>
        public EUniverse ConnectedUniverse => steamClient.ConnectedUniverse;
        /// <summary>
        /// Gets a value indicating whether this instance is connected to the remote CM server.
        /// </summary>
        public bool IsConnected => steamClient.IsConnected;
        /// <summary>
        ///  Gets the session token assigned to this client from the AM.
        /// </summary>
        public ulong SessionToken => steamClient.SessionToken;
        /// <summary>
        /// Gets the Steam recommended Cell ID of this client. This value is assigned after a logon attempt has succeeded. This value will be null if the client is logged off of Steam.
        /// </summary>
        public uint? CellID => steamClient.CellID;
        /// <summary>
        /// Gets the session ID of this client. This value is assigned after a logon attempt has succeeded. This value will be null if the client is logged off of Steam.
        /// </summary>
        public int? SessionID => steamClient.SessionID;
        /// <summary>
        /// Gets the SteamID of this client. This value is assigned after a logon attempt has succeeded. This value will be null if the client is logged off of Steam.
        /// </summary>
        public SteamID SteamID => steamClient.SteamID;
        /// <summary>
        /// Gets or sets the connection timeout used when connecting to the Steam server. 
        /// The default value is 5 seconds.
        /// </summary>
        public TimeSpan ConnectionTimeout
        {
            get => steamClient.ConnectionTimeout;
            set => steamClient.ConnectionTimeout = value;
        }
        /// <summary>
        /// Gets or sets the network listening interface. Use this for debugging only. 
        /// For your convenience, you can use <see cref="NetHookNetworkListener"/> class.
        /// </summary>
        public IDebugNetworkListener DebugNetworkListener
        {
            get => steamClient.DebugNetworkListener;
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
        /// <param name="cmServer">
        /// The <see cref="IPEndPoint"/> of the CM server to connect to. If null, SteamKit will 
        /// randomly select a CM server from its internal list.
        /// </param>
        public void Connect(IPEndPoint cmServer = null) => steamClient.Connect(cmServer);
        /// <summary>
        /// Disconnects this client.
        /// </summary>
        public void Disconnect() => steamClient.Disconnect();
        /// <summary>
        /// Returns the list of servers matching the given type
        /// </summary>
        /// <param name="type">Server type requested</param>
        /// <returns>List of server endpoints</returns>
        public List<IPEndPoint> GetServersOfType(EServerType type) => steamClient.GetServersOfType(type);
        /// <summary>
        /// Sends the specified client message to the server. This method automatically assigns the correct SessionID and SteamID of the message.
        /// </summary>
        /// <param name="msg">The client message to send.</param>
        public void Send(IClientMsg msg) => steamClient.Send(msg);

        /// <summary>
        /// Adds a new handler to the internal list of message handlers.
        /// </summary>
        /// <param name="handler">The handler to add.</param>
        /// <exception cref="InvalidOperationException">
        /// A handler of that type is already registered.
        /// </exception>
        public void AddHandler(ClientMsgHandler handler) => steamClient.AddHandler(handler);
        /// <summary>
        /// Removes a registered handler by name.
        /// </summary>
        /// <param name="handler">The handler name to remove.</param>
        public void RemoveHandler(Type handler) => steamClient.RemoveHandler(handler);
        /// <summary>
        /// Removes a registered handler.
        /// </summary>
        /// <param name="handler">The handler to remove.</param>
        public void RemoveHandler(ClientMsgHandler handler) => steamClient.RemoveHandler(handler);
        /// <summary>
        /// Returns a registered handler.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the handler to cast to. Must derive from <see cref="ClientMsgHandler"/>.
        /// </typeparam>
        /// <returns>A registered handler on success, or null if the handler could not be found.</returns>
        public T GetHandler<T>() where T : ClientMsgHandler => steamClient.GetHandler<T>();
        /// <summary>
        /// Gets the next callback object in the queue. This function does not dequeue the 
        /// callback, you must call <see cref="FreeLastCallback"/> after processing it.
        /// </summary>
        /// <returns>The next callback in the queue, or null if no callback is waiting.</returns>
        public ICallbackMsg GetCallback() => steamClient.GetCallback();
        /// <summary>
        /// Gets the next callback object in the queue, and optionally frees it.
        /// </summary>
        /// <param name="freeLast">if set to true this function also frees the last callback if one existed.</param>
        /// <returns>The next callback in the queue, or null if no callback is waiting.</returns>
        public ICallbackMsg GetCallback(bool freeLast) => steamClient.GetCallback(freeLast);
        /// <summary>
        /// Blocks the calling thread until a callback object is posted to the queue. This 
        /// function does not dequeue the callback, you must call <see cref="FreeLastCallback"/> after 
        /// processing it.
        /// </summary>
        /// <returns>The callback object from the queue.</returns>
        public ICallbackMsg WaitForCallback() => steamClient.WaitForCallback();
        /// <summary>
        /// Blocks the calling thread until a callback object is posted to the queue, or 
        /// null after the timeout has elapsed. This function does not dequeue the callback, 
        /// you must call <see cref="FreeLastCallback"/> after processing it.
        /// </summary>
        /// <param name="timeout">The length of time to block.</param>
        /// <returns>
        /// A callback object from the queue if a callback has been posted, or null if the timeout has elapsed.
        /// </returns>
        public ICallbackMsg WaitForCallback(TimeSpan timeout) => steamClient.WaitForCallback(timeout);
        /// <summary>
        /// Blocks the calling thread until a callback object is posted to the queue, and optionally frees it.
        /// </summary>
        /// <param name="freeLast">if set to true this function also frees the last callback.</param>
        /// <returns>The callback object from the queue.</returns>
        public ICallbackMsg WaitForCallback(bool freeLast) => steamClient.WaitForCallback(freeLast);
        /// <summary>
        /// Blocks the calling thread until a callback object is posted to the queue, and optionally frees it.
        /// </summary>
        /// <param name="freeLast">if set to true this function also frees the last callback.</param>
        /// <param name="timeout">The length of time to block.</param>
        /// <returns>
        /// A callback object from the queue if a callback has been posted, or null if the timeout has elapsed.
        /// </returns>
        public ICallbackMsg WaitForCallback(bool freeLast, TimeSpan timeout) => steamClient.WaitForCallback(freeLast, timeout);
        /// <summary>
        /// Blocks the calling thread until the queue contains a callback object. Returns all callbacks, and optionally frees them.
        /// </summary>
        /// <param name="freeLast">if set to true this function also frees all callbacks.</param>
        /// <param name="timeout">The length of time to block.</param>
        /// <returns>All current callback objects in the queue.</returns>
        public IEnumerable<ICallbackMsg> GetAllCallbacks(bool freeLast, TimeSpan timeout) => steamClient.GetAllCallbacks(freeLast, timeout);
        /// <summary>
        /// Frees the last callback in the queue.
        /// </summary>
        public void FreeLastCallback() => steamClient.FreeLastCallback();
        /// <summary>
        /// Returns the next available JobID for job based messages. This function is thread-safe.
        /// </summary>
        /// <returns>The next available JobID.</returns>
        public JobID GetNextJobID() => steamClient.GetNextJobID();
        /// <summary>
        /// Posts a callback to the queue. This is normally used directly by client message handlers.
        /// </summary>
        /// <param name="msg">The message.</param>
        public void PostCallback(CallbackMsg msg) => steamClient.PostCallback(msg);
    }
}
