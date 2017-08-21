using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpErrorException : Exception
    {
        public HttpErrorException(string message, string stackTrace) : base(message)
        {
            this.stackTrace = stackTrace;
        }

        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }

        readonly string stackTrace;
        public override string StackTrace => stackTrace;
    }
}
