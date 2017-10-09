using System.Net;
using System.Threading.Tasks;
using SteamKit2;
using static SteamKit2.SteamClient;
using static SteamKit2.SteamUser;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    interface ISteamClientAdapter : ISteamClient
    {
        /// <summary>
        /// Gets a value indicating whether this instance is logged on to the remote CM server.
        /// </summary>
        bool IsLoggedOn { get; }
        /// <summary>
        /// Gets or sets the network listening interface.
        /// </summary>
        ProgressDebugNetworkListener ProgressDebugNetworkListener { get; set; }

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
        Task<ConnectedCallback> ConnectAsync(IPEndPoint cmServer = null);
        /// <summary>
        /// Logs the client into the Steam3 network. The client should already have been
        /// connected at this point. Results are returned in a <see cref="SteamUser.LoggedOnCallback"/>.
        /// </summary>
        /// <param name="details">The details to use for logging on.</param>
        /// <exception cref="ArgumentNullException">
        /// No logon details were provided.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Username or password are not set within details.
        /// </exception>
        Task<LoggedOnCallback> LogOnAsync(LogOnDetails details);
        /// <summary>
        /// Returns a registered handler for <see cref="SteamUserStats"/>.
        /// </summary>
        /// <returns>A registered handler on success, or null if the handler could not be found.</returns>
        ISteamUserStats GetSteamUserStats();
    }
}