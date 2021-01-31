using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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

        public ActionResult<Diesel> Diesel(string id) => Get<Diesel>(id);
        public ActionResult<RectifierAndBattery> RectifierAndBattery(string id) => Get<RectifierAndBattery>(id);
        public ActionResult<AirConditioner> AirConditioner(string id) => Get<AirConditioner>(id);
        public ActionResult<Ups> Ups(string id) => Get<Ups>(id);
        public ActionResult<Compressor> Compressor(string id) => Get<Compressor>(id);
        public ActionResult<GasCable> GasCable(string id) => Get<GasCable>(id);

        private Eq Get<Eq>(string id) where Eq : Equipment => db.Find<Eq>(e => e.Id == id && e.Deleted != true).FirstOrDefault();

        [HttpPost]
        public IActionResult Diesel(Diesel model) => Save(model);

        [HttpPost]
        public IActionResult RectifierAndBattery(RectifierAndBattery model) => Save(model);

        [HttpPost]
        public IActionResult AirConditioner(AirConditioner model) => Save(model);

        [HttpPost]
        public IActionResult Ups(Ups model) => Save(model);

        [HttpPost]
        public IActionResult Compressor(Compressor model) => Save(model);

        [HttpPost]
        public IActionResult GasCable(GasCable model)
        {
            model.Index = GasCableNewIndex(model.Center).Value;
            return Save(model);
        }

        private IActionResult Save<Eq>(Eq eq) where Eq : Equipment
        {
            db.Save(eq);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteDiesel(string id) => Delete<Diesel>(id);

        [HttpDelete]
        public IActionResult DeleteRectifierAndBattery(string id) => Delete<RectifierAndBattery>(id);

        [HttpDelete]
        public IActionResult DeleteAirConditioner(string id) => Delete<AirConditioner>(id);

        [HttpDelete]
        public IActionResult DeleteUps(string id) => Delete<Ups>(id);

        [HttpDelete]
        public IActionResult DeleteCompressor(string id)
        {
            if (db.Count<GasCable>(g => g.ConnectedCompressor == id && !g.Deleted) > 0)
                return BadRequest("لطفا ابتدا کابلهای هواخور متصل به این کمپرسور را حذف کنید.");
            return Delete<Compressor>(id);
        }

        [HttpDelete]
        public IActionResult DeleteGasCable(string id) => Delete<GasCable>(id);

        private IActionResult Delete<Eq>(string id) where Eq : Equipment
        {
            db.UpdateOne(d => d.Id == id, Builders<Eq>.Update.Set(d => d.Deleted, true));
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
