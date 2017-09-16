using System;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    public sealed class SteamClientApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified error 
        /// message.
        /// </summary>
        internal SteamClientApiException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        internal SteamClientApiException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified result.
        /// </summary>
        internal SteamClientApiException(string message, EResult result) : base(message)
        {
            Result = result;
        }

        public EResult? Result { get; }
    }
}
