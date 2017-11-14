using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using toofz.NecroDancer.Leaderboards.Logging;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class OAuth2Handler : DelegatingHandler
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(OAuth2Handler));
        private static readonly AuthenticationHeaderValue BearerHeader = new AuthenticationHeaderValue("Bearer");

        public OAuth2Handler(string userName, string password) : this(userName, password, Log) { }

        internal OAuth2Handler(string userName, string password, ILog log)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException($"'{nameof(userName)}' is null or empty.", nameof(userName));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"'{nameof(password)}' is null or empty.", nameof(password));

            this.userName = userName;
            this.password = password;
            this.log = log;
        }

        private readonly string userName;
        private readonly string password;
        private readonly ILog log;

        internal OAuth2BearerToken BearerToken { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddBearerToken();
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Debug.Assert(response.Headers.WwwAuthenticate.Contains(BearerHeader), "Did not receive 'WWW-Authenticate: Bearer' header.");

                BearerToken = await AuthenticateAsync(response.RequestMessage.RequestUri, cancellationToken).ConfigureAwait(false);
                AddBearerToken();
                // Not sure why the request doesn't need to be cloned even though it's been sent before.
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return response;

            void AddBearerToken()
            {
                if (BearerToken != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken.AccessToken);
                }
            }
        }

        private async Task<OAuth2BearerToken> AuthenticateAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            var authUri = new Uri(requestUri, "/token");
            log.Info($"Authenticating to '{authUri}'...");

            var loginData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "userName", userName },
                { "password", password },
            };
            var content = new FormUrlEncodedContent(loginData);

            var response = await PostAsync(authUri, content, cancellationToken).ConfigureAwait(false);

            var bearerToken = await response.Content.ReadAsAsync<OAuth2BearerToken>().ConfigureAwait(false);
            if (bearerToken == null ||
                !((bearerToken.TokenType == "bearer") &&
                  (bearerToken.UserName == userName)))
            {
                throw new InvalidDataException("Did not receive a valid bearer token.");
            }

            return bearerToken;
        }

        private Task<HttpResponseMessage> PostAsync(Uri requestUri, FormUrlEncodedContent content, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };

            return SendAsync(request, cancellationToken);
        }
    }
}
