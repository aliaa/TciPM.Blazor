using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace TciPM.Blazor.Client
{
    public static class HttpClientExtensions
    {
        static readonly JsonSerializerOptions Options;
        static HttpClientExtensions()
        {
            Options = new JsonSerializerOptions();
            Options.Converters.Add(new ObjectIdJsonConverter());
            Options.Converters.Add(new JsonStringEnumConverter());
        }

        public static Task<T> GetFromJsonAsync<T>(this HttpClient client, string? requestUri, CancellationToken cancellationToken = default)
        {
            return HttpClientJsonExtensions.GetFromJsonAsync<T>(client, requestUri, Options, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string? requestUri, T value, CancellationToken cancellationToken = default)
        {
            return HttpClientJsonExtensions.PostAsJsonAsync(client, requestUri, value, Options, cancellationToken);
        }

        public static Task<T> ReadFromJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken = default)
        {
            return HttpContentJsonExtensions.ReadFromJsonAsync<T>(content, Options, cancellationToken);
        }
    }
}
