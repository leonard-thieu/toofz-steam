using System;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    public sealed class SteamCommunityDataApiTransientFaultHandler : TransientFaultHandlerBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SteamCommunityDataApiTransientFaultHandler));

        private static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        private static readonly RetryPolicy RetryPolicy = SteamCommunityDataApiTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

        public SteamCommunityDataApiTransientFaultHandler() : base(RetryPolicy) { }
    }
}
