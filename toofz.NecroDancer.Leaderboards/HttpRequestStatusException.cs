using System;
using System.Net;
using System.Net.Http;

namespace toofz.NecroDancer.Leaderboards
{
    public class HttpRequestStatusException : HttpRequestException
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HttpRequestStatusException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code returned by the server for the request.</param>
        /// <param name="requestUri">The URI of the request that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestUri"/> is null.
        /// </exception>
        public HttpRequestStatusException(HttpStatusCode statusCode, Uri requestUri) : this(null, statusCode, requestUri) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestStatusException"/> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="statusCode">The status code returned by the server for the request.</param>
        /// <param name="requestUri">The URI of the request that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestUri"/> is null.
        /// </exception>
        public HttpRequestStatusException(string message, HttpStatusCode statusCode, Uri requestUri) : this(message, statusCode, requestUri, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestStatusException"/> class 
        /// with a specific message that describes the current exception.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="statusCode">The status code returned by the server for the request.</param>
        /// <param name="requestUri">The URI of the request that caused the exception.</param>
        /// <param name="responseContent">The content of the response.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestUri"/> is null.
        /// </exception>
        public HttpRequestStatusException(string message, HttpStatusCode statusCode, Uri requestUri, string responseContent)
            : base(message)
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            StatusCode = statusCode;
            RequestUri = requestUri;
            ResponseContent = responseContent;
        }

        /// <summary>
        /// The status code returned by the server for the request.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        /// <summary>
        /// The URI of the request that caused the exception.
        /// </summary>
        public Uri RequestUri { get; }
        /// <summary>
        /// The content of the response.
        /// </summary>
        public string ResponseContent { get; }
    }
}
