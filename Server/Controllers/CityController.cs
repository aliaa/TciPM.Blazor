using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciCommon.Server;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    [Authorize(nameof(Permission.ShowCenters))]
    public class CityController : BaseController
    {
        public CityController(ProvinceDBs dbs) : base(dbs) { }

        public ActionResult<List<City>> List()
        {
            return Cities.ToList();
        }

        public ActionResult<City> Item(string id)
        {
            return db.FindById<City>(id);
        }

        [HttpPost]
        [Authorize(nameof(Permission.ChangeCities))]
        public IActionResult Add(City city)
        {
            city.Province = Province.Id;
            db.Save(city);
            return Ok();
        }

        [HttpPost]
        [Authorize(nameof(Permission.ChangeCities))]
        public IActionResult Edit([FromRoute] string id, [FromBody] City city)
        {
            city.Province = Province.Id;
            city.Id = id;
            db.Save(city);
            return Ok();
        }

        class AggResult
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            [BsonRepresentation(BsonType.ObjectId)]
            public string City { get; set; }
            public bool IsOnTime { get; set; }
            public bool IsImportant { get; set; }
        }

        public ActionResult<List<CentersAggregatedData>> ListWithReports()
        {
            var centerPmsCount = db.Aggregate<EquipmentsPM>()
                .Group(id => id.CenterId, g => new { Id = g.Key, Count = g.Count() })
                .Lookup(nameof(CommCenter), "Id", "_id", "Center")
                .Project("{ Count:1, Center: {$arrayElemAt: [\"$Center\", 0]} }")
                .Group("{ _id: \"$Center.City\", Count: {$sum: \"$Count\" } }")
                .Match("{_id: {$ne: null} }")
                .ToEnumerable()
                .ToDictionary(k => k["_id"].AsObjectId.ToString(), v => v["Count"].AsInt32);

            var dailyPmsCount = db.Aggregate<DailyPM2>()
                .Group(id => id.CenterId, g => new { Id = g.Key, Count = g.Count() })
                .Lookup(nameof(CommCenter), "Id", "_id", "Center")
                .Project("{ Count:1, Center: {$arrayElemAt: [\"$Center\", 0]} }")
                .Group("{ _id: \"$Center.City\", Count: {$sum: \"$Count\" } }")
                .Match("{_id: {$ne: null} }")
                .ToEnumerable()
                .ToDictionary(k => k["_id"].AsObjectId.ToString(), v => v["Count"].AsInt32);

            var onTimeAgg = db.Aggregate<EquipmentsPM>()
                .Group("{ _id: \"$CenterId\", LastPmDate: {$last: \"$PmDate\"}}")
                .Lookup("CommCenter", "_id", "_id", "Center")
                .Project("{ Center: {$arrayElemAt: [\"$Center\", 0]}, " +
                         "ElapsedDays: {$divide: [{$subtract: [new Date(), \"$LastPmDate\"]}, 86400000]}}")
                .Match("{\"Center.EquipmentsPmEnabled\": true}")
                .Project("{ City: \"$Center.City\", " +
                        "IsOnTime: {$gte: [\"$Center.PMPeriodDays\", \"$ElapsedDays\"]}, " +
                        "IsImportant: {$gt: [\"$Center.ImportanceLevel\", 5]}}")
                .As<AggResult>().ToList();

            var group = onTimeAgg.GroupBy(k => k.City).ToDictionary(k => k.Key);

            var centersCount = db.Aggregate<CommCenterX>()
                .Match(c => c.EquipmentsPmEnabled)
                .Project(c => new { c.Id, c.City, IsImportant = c.ImportanceLevel > 5 })
                .Group(id => new { id.City, id.IsImportant }, g => new { g.Key, Count = g.Count() })
                .Project(x => new { x.Key.City, x.Key.IsImportant, x.Count })
                .ToEnumerable()
                .Where(x => group.ContainsKey(x.City))
                .ToList();

            var list = new List<CentersAggregatedData>();
            foreach (City city in Cities)
            {
                int importantCentersCount = centersCount.Where(x => x.City == city.Id && x.IsImportant).Select(x => x.Count).FirstOrDefault();
                int unimportantCentersCount = centersCount.Where(x => x.City == city.Id && !x.IsImportant).Select(x => x.Count).FirstOrDefault();

                list.Add(new CentersAggregatedData
                {
                    Id = city.Id,
                    Name = city.Name,
                    CentersCount = (int)db.Count<CommCenterX>(c => c.City == city.Id),
                    MoreThan5PriorityCentersCount = (int)db.Count<CommCenterX>(c => c.City == city.Id && c.ImportanceLevel > 5),
                    LessThan5PriorityCentersCount = (int)db.Count<CommCenterX>(c => c.City == city.Id && c.ImportanceLevel <= 5),
                    EquipmentPMsCount = centerPmsCount.ContainsKey(city.Id) ? centerPmsCount[city.Id] : 0,
                    DailyPMsCount = dailyPmsCount.ContainsKey(city.Id) ? dailyPmsCount[city.Id] : 0,
                    CentersOnTimePMPercent = group.ContainsKey(city.Id) && group[city.Id].Count() > 0 ?
                            (int?)Math.Round(group[city.Id].Count(a => a.IsOnTime) * 100f / group[city.Id].Count()) : null,

                    LessThan5PriorityCentersOnTimePM = group.ContainsKey(city.Id) && unimportantCentersCount > 0 ?
                            (int?)Math.Round(group[city.Id].Count(a => a.IsOnTime && !a.IsImportant) * 100f / unimportantCentersCount) : null,

                    MoreThan5PriorityCentersOnTimePM = group.ContainsKey(city.Id) && importantCentersCount > 0 ?
                            (int?)Math.Round(group[city.Id].Count(a => a.IsOnTime && a.IsImportant) * 100f / importantCentersCount) : null,
                });
            }
            return list;
        }
    }
}
