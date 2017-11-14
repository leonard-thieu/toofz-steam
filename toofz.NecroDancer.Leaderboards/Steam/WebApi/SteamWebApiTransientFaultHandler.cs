using System;
using System.Net;
using Microsoft.ApplicationInsights;
using Polly;
using Polly.Retry;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    /// <summary>
    /// Enables automatic retries for requests to Steam Web API on transient faults.
    /// </summary>
    public sealed class SteamWebApiTransientFaultHandler : TransientFaultHandlerBase
    {
        internal static readonly PolicyBuilder RetryStrategy = Policy
            .Handle<HttpRequestStatusException>(ex =>
            {
                // https://partner.steamgames.com/doc/webapi_overview/responses#status_codes
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:         // 408
                    case HttpStatusCode_TooManyRequests:        // 429 - You are being rate limited.
                    case HttpStatusCode.InternalServerError:    // 500 - An unrecoverable error has occurred, please try again.
                    case HttpStatusCode.BadGateway:             // 502
                    case HttpStatusCode.ServiceUnavailable:     // 503 - Server is temporarily unavailable, or too busy to respond. Please wait and try again later.
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
        public SteamWebApiTransientFaultHandler(TelemetryClient telemetryClient) : base(telemetryClient)
        {
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

        protected override RetryPolicy RetryPolicy { get; }
    }
}
