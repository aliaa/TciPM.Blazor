using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TciPM.Blazor.Shared;
using Blazored.Modal;

namespace TciPM.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();
            var address = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "/api/");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = address });
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(options =>
            {
                foreach (string perm in Enum.GetNames(typeof(Permission)))
                    options.AddPolicy(perm, policy => policy.RequireAssertion(context =>
                    {
                        var permClaim = context.User.FindFirst(nameof(Permission));
                        return permClaim != null && permClaim.Value.Contains(perm);
                    }));
                options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin", "true"));
                options.AddPolicy("SuperAdmin", policy => policy.RequireClaim("IsSuperAdmin", "true"));
            });
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
