using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var resp = await GetAsync(requestUri, cancellationToken);
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    nav.NavigateTo("/login");
                    return default(T);
                }
                return await resp.Content.ReadFromJsonAsync<T>(jsonOptions);
            }
            catch(Exception ex)
            {
                throw new Exception("Errr on GET", ex);
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken = default)
        {
            JsonContent content = JsonContent.Create(value, mediaType: null, jsonOptions);
            try
            {
                var resp = await PostAsync(requestUri, content, cancellationToken);
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    nav.NavigateTo("/login");
                return resp;
            }
            catch(Exception ex)
            {
                throw new Exception("Errr on POST", ex);
            }
        }

        public async Task<Res> PostAsJsonAsync<Req,Res>(string requestUri, Req value, CancellationToken cancellationToken = default)
        {
            JsonContent content = JsonContent.Create(value, mediaType: null, jsonOptions);
            try
            {
                var resp = await PostAsync(requestUri, content, cancellationToken);
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    nav.NavigateTo("/login");
                return await resp.Content.ReadFromJsonAsync<Res>(jsonOptions);
            }
            catch (Exception ex)
            {
                throw new Exception("Errr on POST 2", ex);
            }
        }
    }
}
