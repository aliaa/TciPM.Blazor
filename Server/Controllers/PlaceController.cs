using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PlaceController : BaseController
    {
        public PlaceController(ProvinceDBs dbs) : base(dbs) { }

        [HttpGet]
        public ActionResult<List<Province>> ProvinceList()
        {
            return dbs.CommonDb.Find<Province>(_ => true).SortBy(p => p.Name).ToList();
        }

    }
}
