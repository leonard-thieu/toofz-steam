using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    /// <summary>
    /// Wraps an instance of <see cref="HttpMessageHandler"/> so that its <see cref="HttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/> 
    /// method can be tested.
    /// </summary>
    internal sealed class HttpMessageHandlerAdapter : DelegatingHandler
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HttpMessageHandlerAdapter"/> class with a
        /// specific inner handler.
        /// </summary>
        /// <param name="innerHandler">The <see cref="HttpMessageHandler"/> to be tested.</param>
        public HttpMessageHandlerAdapter(HttpMessageHandler innerHandler) : base(innerHandler) { }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// The request was null.
        /// </exception>
        public Task<HttpResponseMessage> PublicSendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return SendAsync(request, cancellationToken);
        }
    }
}
