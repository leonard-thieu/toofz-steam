using System;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards.Steam
{
    public interface IUgcHttpClient
    {
        Task<byte[]> GetUgcFileAsync(
            string url,
            IProgress<long> progress = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}