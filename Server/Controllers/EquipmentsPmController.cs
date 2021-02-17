using System.Collections.Generic;
using System.Linq;
using AliaaCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TciCommon.Models;
using TciPM.Blazor.Server.Models;
using TciPM.Blazor.Shared;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.ViewModels;
using TciCommon.Server;
using System.Reflection;
using System.IO;
using OfficeOpenXml;
using TciPM.Blazor.Shared.Models.Equipments.PM;
using TciPM.Blazor.Shared.Models.Equipments;
using System.Threading.Tasks;

namespace TciPM.Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    [Authorize(nameof(Permission.ShowPMs))]
    public class EquipmentsPmController : BaseController
    {
        private readonly static Dictionary<int, PmSearchVM> registeredSearches = new Dictionary<int, PmSearchVM>();

        public EquipmentsPmController(ProvinceDBs dbs) : base(dbs) { }

        public ActionResult<EquipmentsPM> Item(string id)
        {
            var pm = db.FindById<EquipmentsPM>(id);
            foreach (var dpm in pm.DieselsPM)
                if (dpm.Source == null)
                    dpm.Source = db.FindById<Diesel>(dpm.SourceId);
            foreach (var rpm in pm.RectifiersPM)
                if (rpm.Source == null)
                    rpm.Source = db.FindById<RectifierAndBattery>(rpm.SourceId);
            foreach (var bpm in pm.BatteriesPM)
                if (bpm.Source == null)
                    bpm.Source = db.FindById<RectifierAndBattery>(bpm.SourceId);
            foreach (var upm in pm.UpsPM)
                if (upm.Source == null)
                    upm.Source = db.FindById<Ups>(upm.SourceId);
            return pm;
        }

        public ActionResult<List<CenterNameVM>> CentersToPm()
        {
            var user = GetUser();
            if (!user.Permissions.Contains(Permission.WriteEquipmentPM))
                return Unauthorized();
            var list = new List<CenterNameVM>();
            foreach (var cityId in user.Cities)
            {
                var city = db.FindById<City>(cityId);
                list.AddRange(db.Find<CommCenterX>(c => c.City == cityId && c.EquipmentsPmEnabled)
                    .SortByDescending(c => c.ImportanceLevel).ThenBy(c => c.Name)
                    .ToEnumerable()
                    .Select(c => new CenterNameVM { CityName = city.Name, Name = c.Name, Id = c.Id, PMPeriodDays = c.PMPeriodDays }));
            }
            foreach (var item in list)
                item.ElapsedDaysOfLastPm = CommCenterController.GetDaysLastPM(db, item.Id);
            return list;
        }

        public ActionResult<EquipmentsPM> New(string centerId)
        {
            var center = CommCenterController.GetItem(db, centerId);
            var user = GetUser();
            var pm = new EquipmentsPM { CenterId = centerId };
            if (user.AllowedEquipmentTypes.Contains(EquipmentType.Diesel))
            {
                foreach (var eq in center.Diesels)
                    pm.DieselsPM.Add(new DieselPM(eq));
            }
            if (user.AllowedEquipmentTypes.Contains(EquipmentType.Rectifier))
            {
                foreach (var eq in center.RectifierAndBatteries)
                    pm.RectifiersPM.Add(new RectifierPM(eq));
            }
            if (user.AllowedEquipmentTypes.Contains(EquipmentType.Battery))
            {
                foreach (var eq in center.RectifierAndBatteries)
                    pm.BatteriesPM.Add(new BatteryPM(eq));
            }
            //TODO other pms
            return pm;
        }

        [HttpPost]
        public async Task<ActionResult<List<EquipmentsPmListItemVM>>> List(PmSearchVM search)
        {
            var query = GetPmList(search);
            if (query == null)
                return new List<EquipmentsPmListItemVM>();
            if (search.Limit > 0)
                query = query.Limit(search.Limit);
            else
                query = query.Limit(1000);

            var pmList = await query.ToListAsync();
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
            if (search.City != null || search.Center != null)
            {
                if (search.Center != null)
                    filters.Add(fb.Eq(pm => pm.CenterId, search.Center));
                else
                {
                    var centersFilter = new List<FilterDefinition<EquipmentsPM>>();
                    foreach (var id in db.Find<CommCenterX>(c => c.City == search.City).Project(c => c.Id).ToEnumerable())
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
            if (search.SubmittedUser != null)
            {
                filters.Add(fb.Eq(pm => pm.ReportingUser, search.SubmittedUser));
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

        [HttpPost]
        public ActionResult<int> RegisterSearch(PmSearchVM search)
        {
            int hash = search.GetHashCode() ^ ProvincePrefix.GetHashCode();
            if (registeredSearches.ContainsKey(hash))
                registeredSearches[hash] = search;
            else
                registeredSearches.Add(hash, search);
            return hash;
        }

        public IActionResult ListAsExcelFile(int hash)
        {
            if (!registeredSearches.ContainsKey(hash))
                return NotFound();
            var search = registeredSearches[hash];
            var pms = GetPmList(search)?.ToList();
            if (pms == null)
                return Ok();
            var fileName = GetFileName(search);
            var file = CreateExcelFile(pms);
            registeredSearches.Remove(hash);
            return File(file, "application/octet-stream", fileName);
        }

        private string GetFileName(PmSearchVM search)
        {
            string fileName = "";
            if (search.City != null)
                fileName += db.FindById<City>(search.City).Name;
            else
                fileName += "همه";
            fileName += "-";
            if (search.Center != null)
                fileName += db.FindById<CommCenterX>(search.Center).Name;
            else
                fileName += "همه";
            fileName += ".xlsx";
            return fileName;
        }

        private byte[] CreateExcelFile(List<EquipmentsPM> pms)
        {
            using var memStream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(memStream);
            var citiesDic = Cities.ToDictionary(i => i.Id);
            var centers = db.All<CommCenterX>().ToDictionary(i => i.Id);
            var usersName = GetUsersName();

            // diesels sheet
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("دیزل ها");
            sheet.View.RightToLeft = true;
            sheet.DefaultColWidth = 20;
            int col = 1;
            sheet.SetValue(1, col++, "شهر");
            sheet.SetValue(1, col++, "مرکز");
            sheet.SetValue(1, col++, "کاربر ثبت کننده");
            sheet.SetValue(1, col++, "تاریخ ثبت");
            sheet.SetValue(1, col++, "تاریخ تغییر");
            Dictionary<PropertyInfo, int> columns = new Dictionary<PropertyInfo, int>();
            PropertyInfo[] props = typeof(DieselPM).GetProperties();
            for (int i = 0; i < 3; i++)
            {
                foreach (PropertyInfo p in props)
                {
                    if (!columns.ContainsKey(p))
                        columns.Add(p, col);
                    string dispName = DisplayUtils.DisplayName(p);
                    sheet.SetValue(1, col++, (i + 1) + "-" + dispName);
                }
            }

            sheet.Row(1).Style.Font.Bold = true;
            int row = 2;
            foreach (var pm in pms)
            {
                if (centers.ContainsKey(pm.CenterId))
                {
                    if (citiesDic.ContainsKey(centers[pm.CenterId].City))
                        sheet.SetValue(row, 1, citiesDic[centers[pm.CenterId].City].Name);
                    sheet.SetValue(row, 2, centers[pm.CenterId].Name);
                }
                if (usersName.ContainsKey(pm.ReportingUser))
                    sheet.SetValue(row, 3, usersName[pm.ReportingUser]);
                sheet.SetValue(row, 4, PersianDateUtils.GetPersianDateString(pm.SubmitDate));
                sheet.SetValue(row, 5, PersianDateUtils.GetPersianDateString(pm.EditDate));

                for (int i = 0; i < pm.DieselsPM.Count; i++)
                {
                    int colOffset = i * (props.Length + 1);
                    foreach (PropertyInfo prop in props)
                    {
                        object value = prop.GetValue(pm.DieselsPM[i]);
                        if (value != null)
                            sheet.SetValue(row, columns[prop] + colOffset, value.ToString());
                    }
                }
                row++;
            }


            // rectifiers sheet
            sheet = package.Workbook.Worksheets.Add("یکسوسازها");
            sheet.View.RightToLeft = true;
            sheet.DefaultColWidth = 20;
            col = 1;
            sheet.SetValue(1, col++, "شهر");
            sheet.SetValue(1, col++, "مرکز");
            sheet.SetValue(1, col++, "کاربر ثبت کننده");
            sheet.SetValue(1, col++, "تاریخ ثبت");
            sheet.SetValue(1, col++, "تاریخ تغییر");
            columns = new Dictionary<PropertyInfo, int>();
            props = typeof(RectifierPM).GetProperties();
            for (int i = 0; i < 3; i++)
            {
                foreach (PropertyInfo p in props)
                {
                    if (!columns.ContainsKey(p))
                        columns.Add(p, col);
                    string dispName = DisplayUtils.DisplayName(p);
                    sheet.SetValue(1, col++, (i + 1) + "-" + dispName);
                }
            }

            sheet.Row(1).Style.Font.Bold = true;
            row = 2;
            foreach (var pm in pms)
            {
                if (centers.ContainsKey(pm.CenterId))
                {
                    if (citiesDic.ContainsKey(centers[pm.CenterId].City))
                        sheet.SetValue(row, 1, citiesDic[centers[pm.CenterId].City].Name);
                    sheet.SetValue(row, 2, centers[pm.CenterId].Name);
                }
                if (usersName.ContainsKey(pm.ReportingUser))
                    sheet.SetValue(row, 3, usersName[pm.ReportingUser]);
                sheet.SetValue(row, 4, PersianDateUtils.GetPersianDateString(pm.SubmitDate));
                sheet.SetValue(row, 5, PersianDateUtils.GetPersianDateString(pm.EditDate));

                for (int i = 0; i < pm.RectifiersPM.Count; i++)
                {
                    int colOffset = i * (props.Length + 1);
                    foreach (PropertyInfo prop in props)
                    {
                        object value = prop.GetValue(pm.RectifiersPM[i]);
                        if (value != null)
                            sheet.SetValue(row, columns[prop] + colOffset, value.ToString());
                    }
                }
                row++;
            }

            // batteries sheet
            sheet = package.Workbook.Worksheets.Add("باتریها");
            sheet.View.RightToLeft = true;
            sheet.DefaultColWidth = 20;
            col = 1;
            sheet.SetValue(1, col++, "شهر");
            sheet.SetValue(1, col++, "مرکز");
            sheet.SetValue(1, col++, "کاربر ثبت کننده");
            sheet.SetValue(1, col++, "تاریخ ثبت");
            sheet.SetValue(1, col++, "تاریخ تغییر");
            for (int i = 0; i < 4; i++)
            {
                sheet.SetValue(1, col++, (i + 1) + "-" + DisplayUtils.DisplayName<BatteryPM.BatterySeriesPM>(bs => bs.DistilledWaterAdded));
                sheet.SetValue(1, col++, (i + 1) + "-" + DisplayUtils.DisplayName<BatteryPM.BatterySeriesPM>(bs => bs.Temperature));
                sheet.SetValue(1, col++, (i + 1) + "-" + DisplayUtils.DisplayName<BatteryPM.BatterySeriesPM>(bs => bs.OutputCurrent));
                sheet.SetValue(1, col++, (i + 1) + "-" + DisplayUtils.DisplayName<BatteryPM.BatterySeriesPM>(bs => bs.Description));
                for (int j = 1; j <= 25; j++)
                    sheet.SetValue(1, col++, "سری " + (i + 1) + " -ولتاژ سلول " + j);
                for (int j = 1; j <= 25; j++)
                    sheet.SetValue(1, col++, "سری " + (i + 1) + " -غلظت سلول " + j);
            }
            sheet.Row(1).Style.Font.Bold = true;

            row = 2;
            foreach (var pm in pms)
            {
                if (centers.ContainsKey(pm.CenterId))
                {
                    if (citiesDic.ContainsKey(centers[pm.CenterId].City))
                        sheet.SetValue(row, 1, citiesDic[centers[pm.CenterId].City].Name);
                    sheet.SetValue(row, 2, centers[pm.CenterId].Name);
                }
                if (usersName.ContainsKey(pm.ReportingUser))
                    sheet.SetValue(row, 3, usersName[pm.ReportingUser]);
                sheet.SetValue(row, 4, PersianDateUtils.GetPersianDateString(pm.SubmitDate));
                sheet.SetValue(row, 5, PersianDateUtils.GetPersianDateString(pm.EditDate));
                col = 6;
                for (int i = 0; i < pm.BatteriesPM.Count; i++)
                {
                    for (int j = 0; j < pm.BatteriesPM[i].Series.Count; j++)
                    {
                        sheet.SetValue(row, col++, pm.BatteriesPM[i].Series[j].DistilledWaterAdded);
                        sheet.SetValue(row, col++, pm.BatteriesPM[i].Series[j].Temperature);
                        sheet.SetValue(row, col++, pm.BatteriesPM[i].Series[j].OutputCurrent);
                        sheet.SetValue(row, col++, pm.BatteriesPM[i].Series[j].Description);
                        for (int k = 0; k < pm.BatteriesPM[i].Series[j].Voltages.Length; k++)
                            sheet.SetValue(row, col++, pm.BatteriesPM[i].Series[j].Voltages[k]);
                        for (int k = 0; k < pm.BatteriesPM[i].Series[j].Densities.Length; k++)
                            sheet.SetValue(row, col++, pm.BatteriesPM[i].Series[j].Densities[k]);
                    }
                }
                row++;
            }
            package.Save();
            return memStream.ToArray();
        }

        private Dictionary<string, string> GetUsersName()
        {
            return db.Find<AuthUserX>(_ => true)
                .Project(u => new AuthUserX { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName })
                .ToEnumerable().ToDictionary(u => u.Id, u => u.DisplayName);
        }
    }
}
