using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TciPM.Blazor.Client
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private HttpClient httpClient;
        private ILocalStorageService storage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService storage) : base()
        {
            this.httpClient = httpClient;
            this.storage = storage;
        }

        private async Task<ClaimsPrincipal> GetUser()
        {
            ClaimsIdentity identity;
            var claims = await storage.GetItemAsync<Dictionary<string, string>>("userClaims");
            if (claims != null)
                identity = new ClaimsIdentity(claims.Select(c => new Claim(c.Key, c.Value)), "Cookies");
            else
                identity = new ClaimsIdentity();

            return new ClaimsPrincipal(identity);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(await GetUser());
        }

        public async Task<bool> Login(string username, string password, string province)
        {
            var res = await httpClient.PostAsJsonAsync("Account/Login", new { username, password, province });
            if (res.IsSuccessStatusCode)
            {
                var claims = await res.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                await storage.SetItemAsync("userClaims", claims);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return res.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await storage.RemoveItemAsync("userClaims");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
