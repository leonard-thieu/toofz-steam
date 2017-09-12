using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class ToofzHttpErrorHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="HttpErrorException">
        /// The response returned an <see cref="HttpError"/> object.
        /// </exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var message = $"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).";

                await response.Content.LoadIntoBufferAsync().ConfigureAwait(false);
                var httpError = await response.Content.ReadAsAsync<HttpError>(cancellationToken).ConfigureAwait(false);
                if (httpError.Message != null)
                {
                    throw new HttpErrorException(httpError, response.StatusCode) { RequestUri = request.RequestUri };
                }
            }

            return response;
        }
    }
}
