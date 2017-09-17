using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    static class HttpContentExtensions
    {
        public static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null) { return null; }

            var ms = new MemoryStream();
            await content.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;

            var clone = new StreamContent(ms);

            foreach (var header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            return clone;
        }
    }
}
