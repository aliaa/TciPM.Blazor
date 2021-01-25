using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TciCommon.Server;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.ViewModels;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    [Authorize]
    public class EquipmentsController : BaseController
    {
        public EquipmentsController(ProvinceDBs dbs) : base(dbs) { }

        public ActionResult<Diesel> Diesel(string id) => db.FindById<Diesel>(id);
        public ActionResult<RectifierAndBattery> RectBattery(string id) => db.FindById<RectifierAndBattery>(id);
        public ActionResult<AirConditioner> AirConditioner(string id) => db.FindById<AirConditioner>(id);
        public ActionResult<Ups> Ups(string id) => db.FindById<Ups>(id);
        public ActionResult<Compressor> Compressor(string id) => db.FindById<Compressor>(id);
        public ActionResult<GasCable> GasCable(string id) => db.FindById<GasCable>(id);


        [HttpPost]
        public IActionResult Diesel(Diesel model)
        {
            db.Save(model);
            return Ok();
        }
        [HttpPost]
        public IActionResult RectBattery(RectifierAndBattery model)
        {
            db.Save(model);
            return Ok();
        }
        [HttpPost]
        public IActionResult AirConditioner(AirConditioner model)
        {
            db.Save(model);
            return Ok();
        }
        [HttpPost]
        public IActionResult Ups(Ups model)
        {
            db.Save(model);
            return Ok();
        }
        [HttpPost]
        public IActionResult Compressor(Compressor model)
        {
            db.Save(model);
            return Ok();
        }
        [HttpPost]
        public IActionResult GasCable(GasCable model)
        {
            model.Index = GasCableNewIndex(model.Center).Value;
            db.Save(model);
            return Ok();
        }

        public ActionResult<int> GasCableNewIndex(string centerId) => 
            (int)db.Count<GasCable>(g => g.Center == centerId) + 1;

        public ActionResult<List<TextValue>> Compressors(string centerId) =>
            db.FindGetResults<Compressor>(g => g.Center == centerId)
                .Select(g => new TextValue { Text = g.Name, Value = g.Id })
                .ToList();
    }
}
