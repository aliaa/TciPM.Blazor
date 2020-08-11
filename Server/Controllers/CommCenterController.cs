using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Omu.ValueInjecter;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.ViewModels;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    [Authorize(nameof(Permission.ShowCenters))]
    public class CommCenterController : BaseController
    {
        public CommCenterController(ProvinceDBs dbs) : base(dbs) { }
        
        public ActionResult<List<TextValue>> List(ObjectId cityId)
        {
            return db.Find<CommCenterX>(cc => cc.City == cityId)
                .SortByDescending(c => c.ImportanceLevel)
                .ThenByDescending(c => c.CenterCapacity)
                .ThenBy(c => c.Name)
                .Project(c => new TextValue { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
        }

        public ActionResult<CommCenterVM> Item(ObjectId id)
        {
            var center = Mapper.Map<CommCenterVM>(db.FindById<CommCenterX>(id));
            center.Diesels = db.FindGetResults<Diesel>(d => d.Center == id).ToList();
            center.RectifierAndBatteries = db.FindGetResults<RectifierAndBattery>(rb => rb.Center == id).ToList();
            center.Upses = db.FindGetResults<Ups>(u => u.Center == id).ToList();
            center.AirConditioners = db.FindGetResults<AirConditioner>(a => a.Center == id).ToList();
            return center;
        }

        [HttpPost]
        [Authorize(nameof(Permission.ChangeCenters))]
        public IActionResult Save(CommCenterX center)
        {
            db.Save(center);
            return Ok();
        }

        public ActionResult<List<CommCenterWithReports>> ListWithReports(ObjectId cityId)
        {
            var list = db.Find<CommCenterX>(cc => cc.City == cityId)
                .SortByDescending(c => c.ImportanceLevel)
                .ThenByDescending(c => c.CenterCapacity)
                .ThenBy(c => c.Name)
                .As<CommCenterWithReports>()
                .ToList();

            var cityNames = Cities.ToDictionary(k => k.Id, v => v.Name);
            foreach (var center in list)
            {
                center.CityName = cityNames[center.City];
                if (center.EquipmentsPmEnabled)
                    center.ElapsedDaysOfLastPm = GetDaysLastPM(center.Id);
                center.DieselsCount = (int)db.Count<Diesel>(d => d.Center == center.Id && d.Deleted != true);
                center.BatteryAndRectifiersCount = (int)db.Count<RectifierAndBattery>(rb => rb.Center == center.Id && rb.Deleted != true);
                center.UpsCount = (int)db.Count<Ups>(u => u.Center == center.Id && u.Deleted != true);
                center.NotesCount = (int)db.Count<DailyPM2>(n => n.CenterId == center.Id);
            }
            return list;
        }

        private int GetDaysLastPM(ObjectId centerId)
        {
            DateTime lastPmCreateDate = db.Find<EquipmentsPM>(pm => pm.CenterId == centerId)
                .Project(pm => pm.PmDate).SortByDescending(pm => pm.PmDate).FirstOrDefault();

            return (int)Math.Min(999, Math.Round((DateTime.Now - lastPmCreateDate).TotalDays));
        }

        public ActionResult<List<TextValue>> DailyCentersList(ObjectId cityId)
        {
            return db.Find<CommCenterX>(c => c.DailyPmEnabled && c.City == cityId)
                .Project(c => new TextValue { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
        }
    }
}
