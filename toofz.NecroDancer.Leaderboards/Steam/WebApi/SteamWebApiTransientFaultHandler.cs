using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using toofz.NecroDancer.Leaderboards.Logging;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    public sealed class SteamWebApiTransientFaultHandler : TransientFaultHandlerBase
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(SteamWebApiTransientFaultHandler));

        private static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        private static readonly RetryPolicy RetryPolicy = SteamWebApiTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

        public SteamWebApiTransientFaultHandler() : base(RetryPolicy) { }
    }
}
