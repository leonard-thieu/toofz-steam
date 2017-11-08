using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Provides the base implementation for transient fault handlers.
    /// </summary>
    public abstract class TransientFaultHandlerBase : DelegatingHandler
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TransientFaultHandlerBase"/> class with the specified retry policy.
        /// </summary>
        /// <param name="retryPolicy">
        /// The <see cref="RetryPolicy"/> that executes retries on transient faults.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retryPolicy"/> is null.
        /// </exception>
        protected TransientFaultHandlerBase(RetryPolicy retryPolicy)
        {
            this.retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        }

        private readonly RetryPolicy retryPolicy;

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request"/> is null.
        /// </exception>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isInitialSend = true;

            return retryPolicy.ExecuteAsync(async () =>
            {
                // When retrying, the request must be cloned as the same request object cannot be sent more than once.
                if (!isInitialSend)
                {
                    using (var oldRequest = request)
                    {
                        request = await oldRequest.CloneAsync().ConfigureAwait(false);
                    }
                }

                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    isInitialSend = false;

                    throw new HttpRequestStatusException(response.StatusCode, response.RequestMessage.RequestUri);
                }

                return response;
            }, cancellationToken);
        }
    }
}
