using System;
using System.Net;
using System.Net.Http;

namespace toofz.NecroDancer.Leaderboards
{
    public class HttpRequestStatusException : HttpRequestException
    {
        internal HttpRequestStatusException(HttpStatusCode statusCode) : this(null, statusCode) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestStatusException"/> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        internal HttpRequestStatusException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// The status code returned by the server for the request.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        public Uri RequestUri { get; internal set; }
        public string ResponseContent { get; internal set; }
    }
}
