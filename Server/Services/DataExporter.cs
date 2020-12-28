using AliaaCommon;
using EasyMongoNet;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TciCommon.Models;
using TciCommon.Server;
using TciPM.Blazor.Shared.Models;

namespace TciPM.Blazor.Server.Services
{
    public class DataExporter
    {
        private readonly ProvinceDBs dbs;
        private readonly DataTableFactory tableFactory;

        public DataExporter(ProvinceDBs dbs, DataTableFactory tableFactory)
        {
            this.dbs = dbs;
            this.tableFactory = tableFactory;
        }

        public byte[] ExportProvinceEquipmentsToExcel(Province province)
        {
            var db = dbs[province.Prefix];
            var cities = db.Find<City>(c => c.Province == province.Id).ToEnumerable().ToDictionary(i => i.Id, i => i.Name);
            using (var memory = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pkg = new ExcelPackage(memory))
                {
                    var centerNames = db.All<CommCenterX>().ToDictionary(i => i.Id, i => cities[i.City] + " - " + i.Name);

                    AddSheetForProvinceData<Diesel>(db, pkg, centerNames);
                    AddSheetForProvinceData<RectifierAndBattery>(db, pkg, centerNames, excludes: nameof(RectifierAndBattery.Batteries));
                    AddBatterySheet(db, pkg, centerNames);
                    AddSheetForProvinceData<Ups>(db, pkg, centerNames);
                    pkg.Save();
                }
                return memory.ToArray();
            }
        }

        private void AddSheetForProvinceData<T>(IReadOnlyDbContext db, ExcelPackage pkg, Dictionary<string, string> centerNames, params string[] excludes) where T : Equipment
        {
            var sheet = pkg.Workbook.Worksheets.Add(typeof(T).Name);
            var table = CreateDataTable(db.Find<T>(t => t.Deleted != true).ToEnumerable(), centerNames, excludes);
            sheet.Cells["A1"].LoadFromDataTable(table, true);
        }

        private DataTable CreateDataTable<T>(IEnumerable<T> data, Dictionary<string, string> centerNames, params string[] excludes) where T : Equipment
        {
            var refs = new Dictionary<string, Dictionary<string, string>>();
            refs.Add(nameof(Equipment.Center), centerNames);
            List<string> excludesList = new List<string>(excludes);
            excludesList.Add(nameof(Equipment.Deleted));
            excludesList.Add(nameof(Equipment.Id));
            return tableFactory.Create(data, addIndexColumn: true,
                excludeColumns: excludesList.ToArray(),
                valuesReferenceReplacement: refs);
        }

        private void AddBatterySheet(IReadOnlyDbContext db, ExcelPackage pkg, Dictionary<string, string> centerNames)
        {
            var sheet = pkg.Workbook.Worksheets.Add("Batteries");
            var data = db.Find<RectifierAndBattery>(rb => rb.Deleted != true)
                .Project(rb => new { rb.Center, rb.Batteries }).ToList();
            sheet.SetValue(1, 1, "مرکز");
            for (int i = 0; i < data.Max(d => d.Batteries.Count); i++)
            {
                sheet.SetValue(1, i * 6 + 2, (i + 1) + ". مدل");
                sheet.SetValue(1, i * 6 + 3, (i + 1) + ". ظرفیت");
                sheet.SetValue(1, i * 6 + 4, (i + 1) + ". نوع");
                sheet.SetValue(1, i * 6 + 5, (i + 1) + ". تعداد سلول ها");
                sheet.SetValue(1, i * 6 + 6, (i + 1) + ". تاریخ تولید");
                sheet.SetValue(1, i * 6 + 7, (i + 1) + ". تاریخ نصب");
            }
            for (int row = 0; row < data.Count; row++)
            {
                if (centerNames.ContainsKey(data[row].Center))
                    sheet.SetValue(row + 2, 1, centerNames[data[row].Center]);
                for (int i = 0; i < data[row].Batteries.Count; i++)
                {
                    sheet.SetValue(row + 2, i * 6 + 2, data[row].Batteries[i].Model);
                    sheet.SetValue(row + 2, i * 6 + 3, data[row].Batteries[i].Capacity);
                    sheet.SetValue(row + 2, i * 6 + 4, data[row].Batteries[i].Type);
                    sheet.SetValue(row + 2, i * 6 + 5, (int)data[row].Batteries[i].CellsCount);
                    sheet.SetValue(row + 2, i * 6 + 6, PersianDateUtils.GetPersianDateString(data[row].Batteries[i].ProductionDate));
                    sheet.SetValue(row + 2, i * 6 + 7, PersianDateUtils.GetPersianDateString(data[row].Batteries[i].InstallationDate));
                }
            }
        }

        public string ExportCountryEquipmentsToExcel()
        {
            FileInfo finfo = new FileInfo(Path.GetTempFileName());
            if (finfo.Exists)
                finfo.Delete();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pkg = new ExcelPackage(finfo))
            {
                AddSheetForCountryData<Diesel>(pkg);
                AddSheetForCountryData<RectifierAndBattery>(pkg, nameof(RectifierAndBattery.Batteries));
                AddSheetForCountryData<Ups>(pkg);
                pkg.Save();
            }
            return finfo.FullName;
        }

        private void AddSheetForCountryData<T>(ExcelPackage pkg, params string[] excludes) where T : Equipment
        {
            var sheet = pkg.Workbook.Worksheets.Add(typeof(T).Name);
            DataTable table = new DataTable();
            table.Columns.Add("استان");
            table.Columns.Add("شهر");
            table.Columns.Add("مرکز");
            var provinces = dbs[dbs.Keys.First()].Find<Province>(_ => true).SortBy(p => p.Name).ToEnumerable();
            var excludeColumns = new List<string> { nameof(Equipment.Deleted), nameof(Equipment.Id), nameof(Equipment.Center) };
            excludeColumns.AddRange(excludes);
            foreach (var p in provinces)
            {
                var cities = dbs[p.Prefix].Find<City>(c => c.Province == p.Id).ToEnumerable().ToDictionary(c => c.Id, c => c.Name);
                var centers = dbs[p.Prefix].Find<CommCenterX>(_ => true).ToEnumerable().ToDictionary(c => c.Id);
                var items = dbs[p.Prefix].Find<T>(t => t.Deleted != true).ToList();
                int rowCountBefore = table.Rows.Count;
                tableFactory.Create(table, items, addIndexColumn: true, excludeColumns: excludeColumns.ToArray());
                for (int i = 0; i < items.Count; i++)
                {
                    table.Rows[rowCountBefore + i]["استان"] = p.Name;
                    if (centers.ContainsKey(items[i].Center))
                    {
                        table.Rows[rowCountBefore + i]["مرکز"] = centers[items[i].Center].Name;
                        if (cities.ContainsKey(centers[items[i].Center].City))
                            table.Rows[rowCountBefore + i]["شهر"] = cities[centers[items[i].Center].City];
                    }
                }
            }
            sheet.Cells["A1"].LoadFromDataTable(table, true);
        }
    }
}
