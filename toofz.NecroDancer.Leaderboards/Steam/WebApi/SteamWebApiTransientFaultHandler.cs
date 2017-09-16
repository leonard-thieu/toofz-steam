using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    public sealed class SteamWebApiTransientFaultHandler : DelegatingHandler
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SteamWebApiTransientFaultHandler));

        static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        static readonly RetryPolicy<SteamWebApiTransientErrorDetectionStrategy> RetryPolicy = SteamWebApiTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var retryCount = 0;

            return RetryPolicy.ExecuteAsync(async () =>
            {
                if (retryCount > 0)
                {
                    using (var oldRequest = request)
                    {
                        request = await oldRequest.CloneAsync().ConfigureAwait(false);
                    }
                }
                retryCount++;

                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestStatusException(response.StatusCode);
                }

                return response;
            }, cancellationToken);
        }
    }
}
