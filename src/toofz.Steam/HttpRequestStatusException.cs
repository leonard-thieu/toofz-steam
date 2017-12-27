using System;
using System.Net;
using System.Net.Http;

namespace toofz.Steam
{
    /// <summary>
    /// Exception thrown by HTTP clients that use <see cref="TransientFaultHandler"/>.
    /// </summary>
    public sealed class HttpRequestStatusException : HttpRequestException
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HttpRequestStatusException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code returned by the server for the request.</param>
        /// <param name="requestUri">The URI of the request that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestUri"/> is null.
        /// </exception>
        public HttpRequestStatusException(HttpStatusCode statusCode, Uri requestUri)
        {
            StatusCode = statusCode;
            RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        /// <summary>
        /// The status code returned by the server for the request.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        /// <summary>
        /// The URI of the request that caused the exception.
        /// </summary>
        public Uri RequestUri { get; }
    }
}
