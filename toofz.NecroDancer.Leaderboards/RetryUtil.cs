using System;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// An exponential backoff sleep duration provider.
    /// </summary>
    public static class RetryUtil
    {
        #region https://topaz.codeplex.com/

        /*
         * Copyright (c) Microsoft Corporation.
         * 
         * ========================================================================================
         * Microsoft patterns & practices (http://microsoft.com/practices)
         * TRANSIENT FAULT HANDLING APPLICATION BLOCK
         * ========================================================================================
         * 
         * Copyright (c) Microsoft.  All rights reserved.
         * Microsoft would like to thank its contributors, a list
         * of whom are at http://aka.ms/entlib-contributors
         * 
         * Licensed under the Apache License, Version 2.0 (the "License"); you
         * may not use this file except in compliance with the License. You may
         * obtain a copy of the License at
         * 
         * http://www.apache.org/licenses/LICENSE-2.0
         * 
         * Unless required by applicable law or agreed to in writing, software
         * distributed under the License is distributed on an "AS IS" BASIS,
         * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
         * implied. See the License for the specific language governing permissions
         * and limitations under the License.
         */

        /// <summary>
        /// Creates a sleep duration provider that calculates the exponential delay between retries with a random jitter.
        /// </summary>
        /// <param name="minBackoff">The minimum backoff time.</param>
        /// <param name="maxBackoff">The maximum backoff time.</param>
        /// <param name="deltaBackoff">The value that will be used to calculate a random delta in the exponential delay between retries.</param>
        /// <returns>
        /// A sleep duration provider that calcuates the duration to sleep until the next retry.
        /// </returns>
        public static Func<int, TimeSpan> GetExponentialBackoff(TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
        {
            return (int currentRetryCount) => GetExponentialBackoff(currentRetryCount, minBackoff, maxBackoff, deltaBackoff);
        }

        private static readonly Random Jitterer = new Random();

        // TODO: Add support for Retry-After header.
        /// <summary>
        /// Calculates the exponential delay between retries with a random jitter.
        /// </summary>
        /// <param name="currentRetryCount">The current retry count.</param>
        /// <param name="minBackoff">The minimum backoff time.</param>
        /// <param name="maxBackoff">The maximum backoff time.</param>
        /// <param name="deltaBackoff">The value that will be used to calculate a random delta in the exponential delay between retries.</param>
        /// <returns>
        /// The duration to sleep until the next retry.
        /// </returns>
        private static TimeSpan GetExponentialBackoff(int currentRetryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
        {
            var delta = (int)((Math.Pow(2.0, currentRetryCount) - 1.0) * Jitterer.Next((int)(deltaBackoff.TotalMilliseconds * 0.8), (int)(deltaBackoff.TotalMilliseconds * 1.2)));
            var interval = (int)Math.Min(checked(minBackoff.TotalMilliseconds + delta), maxBackoff.TotalMilliseconds);

            return TimeSpan.FromMilliseconds(interval);
        }

        #endregion
    }
}
