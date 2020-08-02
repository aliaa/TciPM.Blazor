using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AliaaCommon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Omu.ValueInjecter;
using TciCommon.Models;
using TciPM.Blazor.Server.Models;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.ViewModels;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseController
    {
        public AccountController(ProvinceDBs dbs) : base(dbs) { }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ClientAuthUser>> Login(LoginVM model)
        {
            if (model == null || !dbs.Keys.Contains(model.Province))
                return Unauthorized();
            var db = dbs[model.Province];
            var user = AuthUserX.CheckAuthentication(db, model.Username, model.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, model.Username),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(nameof(Province), model.Province)
                };
                if (user.IsAdmin)
                    claims.Add(new Claim("IsAdmin", "true"));

                var perms = new StringBuilder();
                foreach (var perm in user.Permissions)
                    perms.Append(perm).Append(",");
                claims.Add(new Claim(nameof(Permission), perms.ToString()));

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var clientUser = Mapper.Map<ClientAuthUser>(user);
                clientUser.ProvincePrefix = model.Province;
                return clientUser;
            }
            return Unauthorized();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties { IsPersistent = false });
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            var user = GetUser();
            if (user != null)
            {
                if (AuthUserDBExtention.GetHash(model.CurrentPassword) == user.HashedPassword)
                {
                    if (model.NewPassword == model.RepeatNewPassword)
                    {
                        user.Password = model.NewPassword;
                        db.Save(user);
                        return Ok();
                    }
                    else
                        return BadRequest("رمز جدید و تکرار آن باهم برابر نیستند.");
                }
                else
                    return BadRequest("رمز فعلی اشتباه میباشد.");
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<TextValue>> UsersWithPermission(Permission permission)
        {
            var list = new List<TextValue>();
            var currentUser = GetUser();
            if (currentUser.HasPermission(permission))
                list.Add(new TextValue { Value = currentUser.Id.ToString(), Text = currentUser.DisplayName + " (خودم)" });
            list.AddRange(db.Find<AuthUserX>(u => u.Permissions.Contains(permission) && u.Id != currentUser.Id)
                .SortBy(u => u.LastName).ThenBy(u => u.FirstName).ToEnumerable()
                .Select(u => new TextValue { Text = u.DisplayName, Value = u.Id.ToString() }));
            return list;
        }

        [Authorize(nameof(Permission.ManageUsers))]
        [HttpPost]
        public IActionResult Add(NewUserVM user)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (db.Any<AuthUserX>(u => u.Username == user.Username))
                return BadRequest("نام کاربری قبلا موجود است!");
            return Ok();
        }
    }
}
