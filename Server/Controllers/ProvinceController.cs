using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProvinceController : BaseController
    {
        public ProvinceController(ProvinceDBs dbs) : base(dbs) { }

        [HttpGet]
        public ActionResult<List<Province>> List()
        {
            return dbs.CommonDb.Find<Province>(_ => true).SortBy(p => p.Name).ToList();
        }
    }
}
