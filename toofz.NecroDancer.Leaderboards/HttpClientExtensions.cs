using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient httpClient,
            string requestUri,
            T value,
            CancellationToken cancellationToken)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return httpClient.PostAsync(requestUri, content, cancellationToken);
        }
    }
}
