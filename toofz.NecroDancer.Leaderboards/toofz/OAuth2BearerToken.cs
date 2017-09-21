using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    [DataContract]
    sealed class OAuth2BearerToken
    {
        [DataMember(Name = "access_token", IsRequired = true)]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type", IsRequired = true)]
        public string TokenType { get; set; }
        [DataMember(Name = "expires_in", IsRequired = true)]
        public long ExpiresIn { get; set; }
        [DataMember(Name = "userName", IsRequired = true)]
        public string UserName { get; set; }

        [DataMember(Name = ".issued", IsRequired = true)]
        public string Issued { get; set; }
        [DataMember(Name = ".expires", IsRequired = true)]
        public string Expires { get; set; }
    }
}
