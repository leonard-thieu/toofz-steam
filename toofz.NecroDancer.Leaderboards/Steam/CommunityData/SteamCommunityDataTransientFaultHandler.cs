using System;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    /// <summary>
    /// Enables automatic retries for requests to Steam Community Data on transient faults.
    /// </summary>
    public sealed class SteamCommunityDataTransientFaultHandler : TransientFaultHandlerBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SteamCommunityDataTransientFaultHandler));

        private static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        private static readonly RetryPolicy RetryPolicy = SteamCommunityDataTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

        /// <summary>
        /// Initializes an instance of the <see cref="SteamCommunityDataTransientFaultHandler"/> class.
        /// </summary>
        public SteamCommunityDataTransientFaultHandler() : base(RetryPolicy) { }
    }
}