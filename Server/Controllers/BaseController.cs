using EasyMongoNet;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TciCommon.Models;
using TciPM.Blazor.Server.Models;
using TciPM.Blazor.Shared;
using TciCommon.Server;

namespace TciPM.Blazor.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ProvinceDBs dbs;

        public BaseController(ProvinceDBs dbs)
        {
            this.dbs = dbs;
            dbs.CommonDb.GetUserNameFunc = () => Username;
            foreach (var k in dbs.Keys)
                dbs[k].GetUserNameFunc = () => Username;
        }

        protected string ProvincePrefix => HttpContext.User.FindFirst(nameof(Province)).Value;

        protected Province Province => dbs.CommonDb.FindFirst<Province>(p => p.Prefix == ProvincePrefix);

        protected IDbContext db => dbs[ProvincePrefix];

        protected string Username => HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        protected string UserId => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

        protected IEnumerable<Permission> UserPermissions
        {
            get
            {
                if (User == null)
                    return Enumerable.Empty<Permission>();
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == nameof(Permission));
                if (claim == null)
                    return Enumerable.Empty<Permission>();
                return claim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => (Permission)Enum.Parse(typeof(Permission), c));
            }
        }

        protected AuthUserX GetUser() => db.FindById<AuthUserX>(UserId);

        protected IEnumerable<City> Cities => db.Find<City>(c => c.Province == Province.Id).SortBy(c => c.Name).ToEnumerable();
    }
}
