using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EasyMongoNet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TciPM.Blazor.Shared;

namespace TciPM.Blazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IDbContext db;

        public AccountController(IDbContext db)
        {
            this.db = db;
        }

        public class LoginData
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<Dictionary<string, string>> Login(LoginData loginData)
        {
            if (loginData != null && loginData.username == "admin" && loginData.password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginData.username),
                    new Claim(ClaimTypes.Name, "Ali"),
                    new Claim(ClaimTypes.Surname, "Aboutalebi"),
                    new Claim(nameof(Permission), "ManageUsers,EditData"),
                    new Claim("IsAdmin", "true")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                return claims.ToDictionary(k => k.Type, v => v.Value);
            }
            return Unauthorized();
        }
    }
}
