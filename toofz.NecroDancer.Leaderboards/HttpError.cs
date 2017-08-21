namespace toofz.NecroDancer.Leaderboards
{
    public sealed class HttpError
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }

        public HttpErrorException ToHttpErrorException()
        {
            return new HttpErrorException(Message, StackTrace)
            {
                ExceptionMessage = ExceptionMessage,
                ExceptionType = ExceptionType,
            };
        }
    }
}
