using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class HttpRequestMessageExtensions
    {
        public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var clone = new HttpRequestMessage();

            clone.Version = request.Version;
            clone.Content = await request.Content.CloneAsync().ConfigureAwait(false);
            clone.Method = request.Method;
            clone.RequestUri = request.RequestUri;

            foreach (var header in request.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            foreach (var prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }

            return clone;
        }
    }
}
