using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    static class HttpContentExtensions
    {
        public static async Task<HttpContent> CloneAsync(this HttpContent httpContent)
        {
            if (httpContent == null) { return null; }

            var ms = new MemoryStream();
            await httpContent.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;

            var clone = new StreamContent(ms);

            foreach (var header in httpContent.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            return clone;
        }

        public static async Task<HttpContent> CloneAsync(this HttpContent httpContent, Stream stream, CancellationToken cancellationToken)
        {
            if (httpContent == null) { return null; }

            var ms = new MemoryStream();
            await stream.CopyToAsync(ms, 81920, cancellationToken).ConfigureAwait(false);
            ms.Position = 0;

            var clone = new StreamContent(ms);

            foreach (var header in httpContent.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            return clone;
        }

        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            if (httpContent == null)
                throw new ArgumentNullException(nameof(httpContent));

            try
            {
                var value = await httpContent.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (JsonSerializationException)
            {
                return default;
            }
        }
    }
}
