using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.ViewModels;
using TciCommon.ServerUtils;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlaceController : BaseController
    {
        public PlaceController(ProvinceDBs dbs) : base(dbs) { }

        public ActionResult<List<TextValue>> ProvinceList()
        {
            return dbs.CommonDb.Find<Province>(_ => true).SortBy(p => p.Name).ToEnumerable()
                .Select(p => new TextValue { Text = p.Name, Value = p.Prefix }).ToList();
        }

        [Authorize(nameof(Permission.ShowCenters))]
        public ActionResult<List<TextValue>> CityList()
        {
            return db.Find<City>(c => c.Province == Province.Id).SortBy(c => c.Name).ToEnumerable()
                .Select(c => new TextValue { Text = c.Name, Value = c.Id.ToString() }).ToList();
        }
    }
}
