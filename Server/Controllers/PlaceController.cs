using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Blazor.Shared;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PlaceController : BaseController
    {
        public PlaceController(ProvinceDBs dbs) : base(dbs) { }

        public ActionResult<List<Province>> ProvinceList()
        {
            return dbs.CommonDb.Find<Province>(_ => true).SortBy(p => p.Name).ToList();
        }

        [Authorize(nameof(Permission.ShowCenters))]
        public ActionResult<List<City>> CityList()
        {
            var province = dbs.CommonDb.FindFirst<Province>(p => p.Prefix == ProvincePrefix);
            return db.Find<City>(c => c.Province == province.Id).SortBy(c => c.Name).ToList();
        }
    }
}
