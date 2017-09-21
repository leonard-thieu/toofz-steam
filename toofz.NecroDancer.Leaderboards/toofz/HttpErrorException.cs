using System;
using System.Net;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class HttpErrorException : HttpRequestStatusException
    {
        public HttpErrorException(HttpError httpError, HttpStatusCode statusCode, Uri requestUri) :
            base(httpError?.Message, statusCode, requestUri)
        {
            if (httpError == null)
                throw new ArgumentNullException(nameof(httpError));

            stackTrace = httpError.StackTrace;
            ExceptionMessage = httpError.ExceptionMessage;
            ExceptionType = httpError.ExceptionType;
        }

        public string ExceptionMessage { get; }
        public string ExceptionType { get; }

        readonly string stackTrace;
        public override string StackTrace => stackTrace;
    }
}
