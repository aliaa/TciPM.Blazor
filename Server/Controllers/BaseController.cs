using EasyMongoNet;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TciCommon.Models;
using TciPM.Blazor.Server.Models;
using TciPM.Blazor.Shared;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ProvinceDBs dbs;

        public BaseController(ProvinceDBs dbs)
        {
            this.dbs = dbs;
        }

        protected string ProvincePrefix => HttpContext.User.FindFirst(nameof(Province)).Value;

        protected IDbContext db => dbs[ProvincePrefix];

        protected ObjectId? UserId
        {
            get
            {
                if (ObjectId.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out ObjectId val))
                    return val;
                return null;
            }
        }

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

        protected AuthUserX GetUser()
        {
            var id = UserId;
            if (id != null)
                return db.FindById<AuthUserX>(id.Value);
            return null;
        }
    }
}
