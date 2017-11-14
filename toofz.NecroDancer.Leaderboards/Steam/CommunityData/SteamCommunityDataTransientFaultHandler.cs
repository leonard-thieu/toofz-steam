using System;
using System.Net;
using Microsoft.ApplicationInsights;
using Polly;
using Polly.Retry;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    /// <summary>
    /// Enables automatic retries for requests to Steam Community Data on transient faults.
    /// </summary>
    public sealed class SteamCommunityDataTransientFaultHandler : TransientFaultHandlerBase
    {
        internal static readonly PolicyBuilder RetryStrategy = Policy
            .Handle<HttpRequestStatusException>(ex =>
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:         // 408
                    case HttpStatusCode.Forbidden:              // 403
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
        /// Initializes an instance of the <see cref="SteamCommunityDataTransientFaultHandler"/> class.
        /// </summary>
        /// <param name="telemetryClient">The client used for reporting telemetry.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="telemetryClient"/> is null.
        /// </exception>
        public SteamCommunityDataTransientFaultHandler(TelemetryClient telemetryClient) : base(telemetryClient)
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