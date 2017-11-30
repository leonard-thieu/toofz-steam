using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam.Workshop
{
    /// <summary>
    /// HTTP client used for downloading user-generated content (UGC) from Steam Workshop.
    /// </summary>
    public interface IUgcHttpClient : IDisposable
    {
        /// <summary>
        /// Gets a UGC file as binary data.
        /// </summary>
        /// <param name="requestUri">The URI to download the UGC file from.</param>
        /// <param name="progress">An optional <see cref="IProgress{T}"/> object used to report download size.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// The UGC file as binary data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestUri"/> is null.
        /// </exception>
        Task<byte[]> GetUgcFileAsync(
            string requestUri,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
    }
}