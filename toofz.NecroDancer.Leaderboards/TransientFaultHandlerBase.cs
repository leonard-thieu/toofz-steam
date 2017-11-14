using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly.Retry;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Provides the base implementation for transient fault handlers.
    /// </summary>
    public abstract class TransientFaultHandlerBase : DelegatingHandler
    {
        protected abstract RetryPolicy RetryPolicy { get; }

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

            return RetryPolicy.ExecuteAsync(async cancellation =>
            {
                // When retrying, the request must be cloned as the same request object cannot be sent more than once.
                if (!isInitialSend)
                {
                    using (var oldRequest = request)
                    {
                        request = await oldRequest.CloneAsync().ConfigureAwait(false);
                    }
                }

                var response = await base.SendAsync(request, cancellation).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    isInitialSend = false;

                    throw new HttpRequestStatusException(response.StatusCode, response.RequestMessage.RequestUri);
                }

                return response;
            }, cancellationToken, continueOnCapturedContext: false);
        }
    }
}
