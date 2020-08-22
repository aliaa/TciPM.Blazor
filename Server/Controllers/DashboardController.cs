using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using TciCommon.Models;
using TciPM.Blazor.Shared.Models;
using TciCommon.ServerUtils;
using TciPM.Blazor.Server.Models;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("SuperAdmin")]
    public class DashboardController : BaseController
    {
        public DashboardController(ProvinceDBs dbs) : base(dbs) { }

        class AggResult
        {
            [BsonId]
            public ObjectId Id { get; set; }
            public DateTime LastPmDate { get; set; }
            public int Period { get; set; }
            public int ImportanceLevel { get; set; }
        }

        public ActionResult<List<ProvinceStatus>> UsageStatus()
        {
            var provinces = db.Find<Province>(p => p.Applications.Contains("PM")).SortBy(p => p.Name).ToEnumerable();
            var list = new List<ProvinceStatus>();
            foreach (var p in provinces)
            {
                var db = dbs[p.Prefix];

                var agg = db.Aggregate<EquipmentsPM>()
                    .Group("{ _id: \"$CenterId\", LastPmDate: {$last: \"$PmDate\"}}")
                    .Lookup(nameof(CommCenter), "_id", "_id", "Center")
                    .Project("{ LastPmDate: 1, Center: {$arrayElemAt: [\"$Center\", 0]} }")
                    .Match("{ \"Center.EquipmentsPmEnabled\": true}")
                    .Project("{ LastPmDate: { $ifNull:[ \"$LastPmDate\", ISODate(\"1970-01-01\")]}, Period: \"$Center.PMPeriodDays\", ImportanceLevel: \"$Center.ImportanceLevel\" }")
                    .As<AggResult>().ToList();

                int centersCount = (int)db.Count<CommCenterX>(c => c.EquipmentsPmEnabled);
                int moreImportantCount = (int)db.Count<CommCenterX>(c => c.EquipmentsPmEnabled && c.ImportanceLevel > 5);
                int lessImportantCount = (int)db.Count<CommCenterX>(c => c.EquipmentsPmEnabled && c.ImportanceLevel <= 5);

                //var onTimeAgg = db.Aggregate<CenterPM>().Group(id => id.CenterId, g => new { Id = g.Key, LastPM = g.Last() })
                //    .Lookup(nameof(CommCenter), "_id", "_id", "Center").ToList();

                var onTimeCount = agg.Where(a => (DateTime.Now - a.LastPmDate).TotalDays > a.Period);

                var status = new ProvinceStatus
                {
                    Name = p.Name,
                    UserCount = (int)db.Count<AuthUserX>(_ => true),
                    CentersCount = (int)db.Count<CommCenterX>(_ => true),
                    MoreThan5PriorityCentersCount = moreImportantCount,
                    LessThan5PriorityCentersCount = lessImportantCount,
                    CentersOnTimePMPercent = centersCount > 0 ?
                            (int?)Math.Round(agg.Count(a => (DateTime.Now - a.LastPmDate).TotalDays < a.Period) * 100f / centersCount) : null,

                    MoreThan5PriorityCentersOnTimePM = moreImportantCount > 0 ?
                            (int?)Math.Round(agg.Count(a => a.ImportanceLevel > 5 && (DateTime.Now - a.LastPmDate).TotalDays < a.Period) * 100f / moreImportantCount) : null,

                    LessThan5PriorityCentersOnTimePM = lessImportantCount > 0 ?
                            (int?)Math.Round(agg.Count(a => a.ImportanceLevel <= 5 && (DateTime.Now - a.LastPmDate).TotalDays < a.Period) * 100f / lessImportantCount) : null,

                    EquipmentPMsCount = (int)db.Count<EquipmentsPM>(_ => true),
                    DailyPMsCount = (int)db.Count<DailyPM2>(_ => true)
                };
                list.Add(status);
            }
            return list;
        }

        public ActionResult<List<ProvinceEquipmentsStatus>> EquipmentsStatus()
        {
            var provinces = db.Find<Province>(p => p.Applications.Contains("PM")).SortBy(p => p.Name).ToEnumerable();
            var list = new List<ProvinceEquipmentsStatus>();
            foreach (var p in provinces)
            {
                var db = dbs[p.Prefix];
                var dieselAgg = db.Aggregate<Diesel>().Match(d => d.Deleted != true).Group(
                    "{ _id: null, " +
                    "Count: { '$sum': 1 }, " +
                    "PowerSum: { $sum: '$Power' } ," +
                    "PermissiveSum: { $sum: { $multiply: ['$Power', '$EfficiencyPercentage', '$AltitudePercentage', '$InitiationPercentage']} }}").FirstOrDefault();

                var rectBattAgg = db.Aggregate<RectifierAndBattery>().Match(r => r.Deleted != true).Group(
                    "{ _id: null, " +
                    "Count: { '$sum': 1 }, " +
                    "SeriesCount: {$sum: { $size: '$Batteries' }}," +
                    "BatteryCapacity: { $sum: { $sum: '$Batteries.Capacity' } }," +
                    "RectifierCapacity: { $sum: { $multiply: ['$EachRectifierCapacity', '$RectifierCount'] } }}").FirstOrDefault();

                var item = new ProvinceEquipmentsStatus { ProvinceName = p.Name };
                if (dieselAgg != null)
                {
                    item.DieselCount = dieselAgg.GetValue("Count").AsInt32;
                    item.DieselPowerSum = dieselAgg.GetValue("PowerSum").AsInt32;
                    item.PermissivePowerSum = (int)Math.Round(dieselAgg.GetValue("PermissiveSum").AsDouble);
                }
                if (rectBattAgg != null)
                {
                    item.RectifierCount = rectBattAgg.GetValue("Count").AsInt32;
                    item.BatterySeriesCount = rectBattAgg.GetValue("SeriesCount").AsInt32;
                    item.BatterySeriesCapacitySum = rectBattAgg.GetValue("BatteryCapacity").AsInt32;
                    item.RectifiersCapacitySum = rectBattAgg.GetValue("RectifierCapacity").AsInt32;
                }
                list.Add(item);
            }

            list.Add(new ProvinceEquipmentsStatus
            {
                ProvinceName = "جمع کل استانها",
                BatterySeriesCapacitySum = list.Sum(s => s.BatterySeriesCapacitySum),
                BatterySeriesCount = list.Sum(s => s.BatterySeriesCount),
                DieselCount = list.Sum(s => s.DieselCount),
                DieselPowerSum = list.Sum(s => s.DieselPowerSum),
                PermissivePowerSum = list.Sum(s => s.PermissivePowerSum),
                RectifierCount = list.Sum(s => s.RectifierCount),
                RectifiersCapacitySum = list.Sum(s => s.RectifiersCapacitySum)
            });
            return list;
        }
    }
}
