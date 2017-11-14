using System;
using System.Net;
using Microsoft.ApplicationInsights;
using Polly;
using Polly.Retry;
using toofz.NecroDancer.Leaderboards.Logging;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    /// <summary>
    /// Enables automatic retries for requests to Steamworks Web API on transient faults.
    /// </summary>
    public sealed class SteamWebApiTransientFaultHandler : TransientFaultHandlerBase
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(SteamWebApiTransientFaultHandler));

        internal static readonly PolicyBuilder RetryStrategy = Policy
            .Handle<HttpRequestStatusException>(ex =>
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:         // 408
                    case HttpStatusCode.InternalServerError:    // 500
                    case HttpStatusCode.BadGateway:             // 502
                    case HttpStatusCode.ServiceUnavailable:     // 503
                    case HttpStatusCode.GatewayTimeout:         // 504
                        return true;
                    default:
                        return false;
                }
            });

        /// <summary>
        /// Initializes an instance of the <see cref="SteamWebApiTransientFaultHandler"/> class.
        /// </summary>
        /// <param name="telemetryClient">The client used for reporting telemetry.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public SteamWebApiTransientFaultHandler(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));

            RetryPolicy = RetryStrategy
                .WaitAndRetryAsync(
                    10,
                    retryAttempt => RetryUtil.GetExponentialBackoff(
                        retryAttempt,
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(20),
                        TimeSpan.FromSeconds(2)),
                    (Action<Exception, TimeSpan>)OnRetry);
        }

        private readonly TelemetryClient telemetryClient;

        protected override RetryPolicy RetryPolicy { get; }

        private void OnRetry(Exception ex, TimeSpan duration)
        {
            telemetryClient.TrackException(ex);
            Log.Debug(() => $"{ex} Retrying in {duration}...");
        }
    }
}
