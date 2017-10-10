using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam
{
    public interface IUgcHttpClient : IDisposable
    {
        Task<byte[]> GetUgcFileAsync(
            string url,
            IProgress<long> progress = default,
            CancellationToken cancellationToken = default);
    }
}