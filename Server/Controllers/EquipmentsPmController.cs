using System.Collections.Generic;
using System.Linq;
using AliaaCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Blazor.Server.Models;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.ViewModels;
using TciPM.Classes;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EquipmentsPmController : BaseController
    {
        public EquipmentsPmController(ProvinceDBs dbs) : base(dbs) { }

        [HttpPost]
        [Authorize(nameof(Permission.ShowPMs))]
        public ActionResult<List<EquipmentsPmListItemVM>> List(PmSearchVM search)
        {
            var query = GetPmList(search);
            if (query == null)
                return new List<EquipmentsPmListItemVM>();
            var pmList = query.Limit(1000).ToList();
            var centers = pmList.GroupBy(pm => pm.CenterId)
                .Select(g => db.FindById<CommCenterX>(g.Key)).Where(c => c != null).ToDictionary(c => c.Id);
            var cities = centers.Values.GroupBy(c => c.City).Select(c => db.FindById<City>(c.Key)).Where(c => c != null).ToDictionary(c => c.Id);
            var users = pmList.GroupBy(pm => pm.ReportingUser)
                .Select(g => db.FindById<AuthUserX>(g.Key)).ToDictionary(u => u.Id);

            return pmList.Select(pm => new EquipmentsPmListItemVM
            {
                Id = pm.Id,
                Center = centers.ContainsKey(pm.CenterId) ? centers[pm.CenterId].Name : "(مرکز حذف شده)",
                City = centers.ContainsKey(pm.CenterId) && cities.ContainsKey(centers[pm.CenterId].City) ? cities[centers[pm.CenterId].City].Name : "",
                EditDate = PersianDateUtils.GetPersianDateString(pm.EditDate),
                SubmitDate = PersianDateUtils.GetPersianDateString(pm.SubmitDate),
                PmDate = PersianDateUtils.GetPersianDateString(pm.PmDate),
                ReportingUser = users[pm.ReportingUser]?.DisplayName ?? "(کاربر حذف شده)",
                TotalRate = pm.TotalRate
            })
            .ToList();
        }

        private IFindFluent<EquipmentsPM, EquipmentsPM> GetPmList(PmSearchVM search)
        {
            var filters = new List<FilterDefinition<EquipmentsPM>>();
            var fb = Builders<EquipmentsPM>.Filter;
            if (ObjectId.TryParse(search.City, out ObjectId cityId))
            {
                if (ObjectId.TryParse(search.Center, out ObjectId centerId))
                    filters.Add(fb.Eq(pm => pm.CenterId, centerId));
                else
                {
                    var centersFilter = new List<FilterDefinition<EquipmentsPM>>();
                    foreach (var id in db.Find<CommCenterX>(c => c.City == cityId).Project(c => c.Id).ToEnumerable())
                        centersFilter.Add(fb.Eq(pm => pm.CenterId, id));
                    if (centersFilter.Count == 0)
                        return null;
                    else if (centersFilter.Count == 1)
                        filters.Add(centersFilter[0]);
                    else if (centersFilter.Count > 1)
                        filters.Add(fb.Or(centersFilter));
                }
            }
            if (!string.IsNullOrEmpty(search.FromDate))
            {
                var fromDate = PersianDateUtils.PersianDateTimeToGeorgian(search.FromDate);
                filters.Add(fb.Gte(pm => pm.SubmitDate, fromDate));
            }
            if (!string.IsNullOrEmpty(search.ToDate))
            {
                var toDate = PersianDateUtils.PersianDateTimeToGeorgian(search.ToDate);
                filters.Add(fb.Lte(pm => pm.SubmitDate, toDate));
            }
            if (ObjectId.TryParse(search.SubmittedUser, out ObjectId userId))
            {
                filters.Add(fb.Eq(pm => pm.ReportingUser, userId));
            }

            var totalFilter = fb.Empty;
            if (filters.Count == 1)
                totalFilter = filters[0];
            else if (filters.Count > 1)
                totalFilter = fb.And(filters);
            return db.Find(totalFilter)
                .Project<EquipmentsPM>(Builders<EquipmentsPM>.Projection
                            .Include(pm => pm.Id)
                            .Include(pm => pm.CenterId)
                            .Include(pm => pm.PmDate)
                            .Include(pm => pm.SubmitDate)
                            .Include(pm => pm.EditDate)
                            .Include(pm => pm.ReportingUser)
                            .Include(pm => pm.TotalRate))
                .SortByDescending(pm => pm.SubmitDate);
        }
    }
}
