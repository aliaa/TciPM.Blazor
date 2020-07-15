using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TciCommon.Models;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.ViewModels;

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

        public async Task<ClientAuthUser> GetUser()
        {
            return await storage.GetItemAsync<ClientAuthUser>("user");
        }

        private async Task<ClaimsPrincipal> GetClaims()
        {
            ClaimsIdentity identity;
            var user = await GetUser();
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(nameof(Province), user.ProvincePrefix)
                };
                var perms = new StringBuilder();
                if(user.IsAdmin)
                {
                    foreach (string p in Enum.GetNames(typeof(Permission)))
                        perms.Append(p).Append(",");
                    claims.Add(new Claim("IsAdmin", "true"));
                }
                else
                {
                    foreach (Permission p in user.Permissions)
                        perms.Append(p).Append(",");
                }
                claims.Add(new Claim(nameof(Permission), perms.ToString()));
                if (user.IsSuperAdmin)
                    claims.Add(new Claim("IsSuperAdmin", "true"));
                identity = new ClaimsIdentity(claims, "Cookies");
            }
            else
                identity = new ClaimsIdentity();

            return new ClaimsPrincipal(identity);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(await GetClaims());
        }

        public async Task<bool> Login(LoginViewModel m)
        {
            var res = await httpClient.PostAsJsonAsync("Account/Login", m);
            if (res.IsSuccessStatusCode)
            {
                var user = await res.Content.ReadFromJsonAsync<ClientAuthUser>();
                await storage.SetItemAsync("user", user);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return res.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await storage.RemoveItemAsync("user");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
