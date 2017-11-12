using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    internal sealed class SteamClientApiTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region Static Members

        public static RetryPolicy<SteamClientApiTransientErrorDetectionStrategy> CreateRetryPolicy(RetryStrategy retryStrategy, ILog log)
        {
            var retryPolicy = new RetryPolicy<SteamClientApiTransientErrorDetectionStrategy>(retryStrategy);
            retryPolicy.Retrying += (s, e) =>
            {
                log.Debug($"{e.LastException.Message} Retrying ({e.CurrentRetryCount}) in {e.Delay}...");
            };

            return retryPolicy;
        }

        #endregion

        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        /// Determines whether the specified exception represents a transient failure that
        /// can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>
        /// true if the specified exception is considered as transient; otherwise, false.
        /// </returns>
        public bool IsTransient(Exception ex)
        {
            if (ex is SteamClientApiException transient)
            {
                return transient.InnerException is TaskCanceledException;
            }
            return false;
        }

        #endregion
    }
}
