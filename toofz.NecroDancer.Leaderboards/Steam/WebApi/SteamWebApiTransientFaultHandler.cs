using System;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    public sealed class SteamWebApiTransientFaultHandler : TransientFaultHandlerBase
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SteamWebApiTransientFaultHandler));

        static readonly RetryStrategy RetryStrategy = new ExponentialBackoff(10, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(2));
        static readonly RetryPolicy RetryPolicy = SteamWebApiTransientErrorDetectionStrategy.CreateRetryPolicy(RetryStrategy, Log);

        public SteamWebApiTransientFaultHandler() : base(RetryPolicy) { }
    }
}
