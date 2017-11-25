using System;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    /// <summary>
    /// Represents errors that occur when using Steam Client API.
    /// </summary>
    public sealed class SteamClientApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified error 
        /// message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SteamClientApiException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public SteamClientApiException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientApiException"/> class with a specified result.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="result">The result returned from the response.</param>
        public SteamClientApiException(string message, EResult result) : base(message)
        {
            Result = result;
        }

        /// <summary>
        /// The result returned from the response, if any.
        /// </summary>
        public EResult? Result { get; }
    }
}
