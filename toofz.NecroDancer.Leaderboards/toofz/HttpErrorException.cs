using System;
using System.Net;
using System.Web.Http;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class HttpErrorException : HttpRequestStatusException
    {
        internal HttpErrorException(HttpError httpError, HttpStatusCode statusCode) :
            base(httpError.Message, statusCode)
        {
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
