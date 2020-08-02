using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TciPM.Blazor.Client
{
    public class HttpClientX : HttpClient
    {
        static readonly JsonSerializerOptions jsonOptions;
        static HttpClientX()
        {
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new ObjectIdJsonConverter());
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
        }

        private NavigationManager nav;

        public HttpClientX(NavigationManager nav)
        {
            this.nav = nav;
        }

        public async Task<T> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage resp;
            try
            {
                resp = await GetAsync(requestUri, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Errr on GET", ex);
            }
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                nav.NavigateTo("/login/out");
                return default;
            }
            else if ((int)resp.StatusCode >= 400)
                throw new HttpResponseException(await resp.Content.ReadAsStringAsync(), (int)resp.StatusCode);
            return await resp.Content.ReadFromJsonAsync<T>(jsonOptions);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken = default)
        {
            JsonContent content = JsonContent.Create(value, mediaType: null, jsonOptions);
            HttpResponseMessage resp;
            try
            {
                resp = await PostAsync(requestUri, content, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Errr on POST", ex);
            }
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                nav.NavigateTo("/login");
            else if ((int)resp.StatusCode >= 400)
                throw new HttpResponseException(await resp.Content.ReadAsStringAsync(), (int)resp.StatusCode);
            return resp;
        }

        public async Task<Res> PostAsJsonAsync<Req, Res>(string requestUri, Req value, CancellationToken cancellationToken = default)
        {
            JsonContent content = JsonContent.Create(value, mediaType: null, jsonOptions);
            HttpResponseMessage resp;
            try
            {
                resp = await PostAsync(requestUri, content, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Errr on POST 2", ex);
            }

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                nav.NavigateTo("/login");
                return default;
            }
            else if ((int)resp.StatusCode >= 400)
                throw new HttpResponseException(await resp.Content.ReadAsStringAsync(), (int)resp.StatusCode);
            return await resp.Content.ReadFromJsonAsync<Res>(jsonOptions);
        }

        [Serializable]
        public class HttpResponseException : Exception
        {
            public int StatusCode { get; private set; } = -1;
            public HttpResponseException(string message, int StatusCode) : base(message)
            {
                this.StatusCode = StatusCode;
            }

            public HttpResponseException(string message, Exception inner) : base(message, inner) { }

            public HttpResponseException() : base() { }

            public HttpResponseException(string message) : base(message) { }
        }
    }
}
