using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LoggingHandler : DelegatingHandler
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(LoggingHandler));

        /// <summary>
        /// Initializes an instance of the <see cref="LoggingHandler"/> class.
        /// </summary>
        public LoggingHandler() : this(Log) { }

        internal LoggingHandler(ILog log)
        {
            this.log = log;
        }

        readonly ILog log;

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken"> A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="request"/> is null.
        /// </exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            log.Debug($"Start download {request.RequestUri}");
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            log.Debug($"End download {response.RequestMessage.RequestUri}");

            return response;
        }
    }
}
