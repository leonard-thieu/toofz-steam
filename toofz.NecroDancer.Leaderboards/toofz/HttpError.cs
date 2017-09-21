using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    [DataContract]
    public sealed class HttpError
    {
        [DataMember(Name = "message", IsRequired = true)]
        public string Message { get; set; }
        [DataMember(Name = "modelState")]
        public HttpError ModelState { get; set; }
        [DataMember(Name = "messageDetail")]
        public string MessageDetail { get; set; }
        [DataMember(Name = "exceptionMessage")]
        public string ExceptionMessage { get; set; }
        [DataMember(Name = "exceptionType")]
        public string ExceptionType { get; set; }
        [DataMember(Name = "stackTrace")]
        public string StackTrace { get; set; }
        [DataMember(Name = "innerException")]
        public HttpError InnerException { get; set; }
    }
}
