using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards
{
    public abstract class TransientFaultHandlerBase : DelegatingHandler
    {
        protected TransientFaultHandlerBase(RetryPolicy retryPolicy)
        {
            this.retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        }

        readonly RetryPolicy retryPolicy;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var retryCount = 0;

            return retryPolicy.ExecuteAsync(async () =>
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
                    throw new HttpRequestStatusException(response.StatusCode, response.RequestMessage.RequestUri);
                }

                return response;
            }, cancellationToken);
        }
    }
}
