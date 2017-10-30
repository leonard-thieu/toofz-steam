using System;
using System.Net;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace toofz.NecroDancer.Leaderboards.Steam.WebApi
{
    internal sealed class SteamWebApiTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region Static Members

        public static RetryPolicy<SteamWebApiTransientErrorDetectionStrategy> CreateRetryPolicy(RetryStrategy retryStrategy, ILog log)
        {
            var retryPolicy = new RetryPolicy<SteamWebApiTransientErrorDetectionStrategy>(retryStrategy);
            retryPolicy.Retrying += (s, e) =>
            {
                log.Debug($"{e.LastException.Message} Retrying ({e.CurrentRetryCount}) in {e.Delay}...");
            };

            return retryPolicy;
        }

        #endregion

        #region ITransientErrorDetectionStrategy Members

        public bool IsTransient(Exception ex)
        {
            if (ex is HttpRequestStatusException transient)
            {
                switch (transient.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}