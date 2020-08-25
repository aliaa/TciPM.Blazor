using EasyMongoNet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TciPM.Blazor.Shared;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Threading.Tasks;
using AliaaCommon;
using TciPM.Blazor.Server.Services;
using TciCommon.ServerUtils;

namespace TciPM.Blazor.Server.Configuration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Events.OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    };
                });

            string permissionClaimName = nameof(Permission);
            services.AddAuthorization(options =>
            {
                foreach (string perm in Enum.GetNames(typeof(Permission)))
                    options.AddPolicy(perm, policy => policy.RequireAssertion(context =>
                    {
                        var permClaim = context.User.Claims.FirstOrDefault(c => c.Type == permissionClaimName);
                        return permClaim != null && permClaim.Value.Contains(perm);
                    }));
                options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin"));
                options.AddPolicy("SuperAdmin", policy => policy.RequireClaim("IsSuperAdmin"));
            });

            services.AddControllersWithViews(config =>
            {
                config.ModelBinderProviders.Insert(0, new ObjectIdModelBinderProvider());
            })
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.Converters.Add(new ObjectIdJsonConverter());
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        // Maintain property names during serialization. See:
                        // https://github.com/aspnet/Announcements/issues/194
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    });
            services.AddRazorPages();
            
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
            services.AddMongDbContext(Configuration);
            services.AddSingleton(sp => new DataTableFactory(sp.GetService<IReadOnlyDbContext>()));
            services.AddSingleton(sp => new DataExporter(sp.GetService<ProvinceDBs>(), sp.GetService<DataTableFactory>()));

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            ConfigureMapper.Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
