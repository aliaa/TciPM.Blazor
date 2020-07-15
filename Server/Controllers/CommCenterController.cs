using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("[controller]/[action]/{id?}")]
    [ApiController]
    [Authorize(nameof(Permission.ShowCenters))]
    public class CommCenterController : BaseController
    {
        public CommCenterController(ProvinceDBs dbs) : base(dbs) { }
        
        public ActionResult<List<CommCenterX>> List(ObjectId cityId)
        {
            return db.Find<CommCenterX>(cc => cc.City == cityId)
                .SortByDescending(c => c.ImportanceLevel)
                .ThenByDescending(c => c.CenterCapacity)
                .ThenBy(c => c.Name)
                .ToList();
        }

        public ActionResult<CommCenterX> Item(ObjectId id)
        {
            return db.FindById<CommCenterX>(id);
        }

        public ActionResult<List<CommCenterWithReports>> CentersWithReports(ObjectId cityId)
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
            DateTime lastPmCreateDate = db.Find<CenterPM>(pm => pm.CenterId == centerId)
                .Project(pm => pm.PmDate).SortByDescending(pm => pm.PmDate).FirstOrDefault();

            return (int)Math.Min(999, Math.Round((DateTime.Now - lastPmCreateDate).TotalDays));
        }
    }
}
