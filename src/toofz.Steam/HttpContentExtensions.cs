using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace toofz.Steam
{
    /// <summary>
    /// Contains extension methods for <see cref="HttpContent"/>.
    /// </summary>
    internal static class HttpContentExtensions
    {
        /// <summary>
        /// Makes a clone of a <see cref="HttpContent"/> instance.
        /// </summary>
        /// <param name="httpContent">The <see cref="HttpContent"/> to clone.</param>
        /// <param name="content">The content to use for the clone.</param>
        /// <returns>
        /// A clone of <paramref name="httpContent"/>, if it is not null; otherwise, null.
        /// </returns>
        public static HttpContent Clone(this HttpContent httpContent, Stream content)
        {
            if (httpContent == null) { return null; }

            var clone = new StreamContent(content);

            foreach (var header in httpContent.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            return clone;
        }

        /// <summary>
        /// Deserializes a JSON response into <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="httpContent">The content to deserialize.</param>
        /// <returns>
        /// An instance of <typeparamref name="T"/> representing the content, if deserialization succeeded; 
        /// otherwise, returns the default value of <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="httpContent"/> is null.
        /// </exception>
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            if (httpContent == null)
                throw new ArgumentNullException(nameof(httpContent));

            var value = await httpContent.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
