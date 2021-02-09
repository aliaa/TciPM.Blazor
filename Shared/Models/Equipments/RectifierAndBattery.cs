using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TciPM.Blazor.Shared.Utils;

namespace TciPM.Blazor.Shared.Models.Equipments
{
    [CollectionSave(WriteLog = true)]
    [BsonIgnoreExtraElements]
    public class RectifierAndBattery : Equipment
    {
        public enum BatteryTypeEnum
        {
            [Display(Name="سیلد")]
            Sild,
            [Display(Name="اسیدی")]
            Acid,
            [Display(Name="سایر")]
            None,
        }

        public enum CellCountEnum
        {
            [Display(Name="2")]
            _2 = 2,
            [Display(Name="4")]
            _4 = 4,
            [Display(Name="8")]
            _8 = 8,
            [Display(Name="12")]
            _12 = 12,
            [Display(Name="24")]
            _24 = 24,
            [Display(Name="25")]
            _25 = 25,
        }

        public class BatterySeries
        {
            [DisplayName("مدل")]
            public string Model { get; set; }

            [DisplayName("ظرفیت")]
            public int Capacity { get; set; }

            [DisplayName("نوع")]
            public BatteryTypeEnum Type { get; set; }

            [Display(Name="تعداد سلولها")]
            [BsonRepresentation(MongoDB.Bson.BsonType.String)]
            public CellCountEnum CellsCount { get; set; } = CellCountEnum._25;

            [DisplayName("تاریخ تولید")]
            public DateTime ProductionDate { get; set; }

            [DisplayName("تاریخ نصب")]
            public DateTime InstallationDate { get; set; }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("مدل ").Append(Model).Append(", ظرفیت ").Append(Capacity)
                    .Append(", نوع ").Append(UtilsX.DisplayName(Type))
                    .Append(", تعداد سلولها ").Append((int)CellsCount);
                return sb.ToString();
            }
        }

        public List<BatterySeries> Batteries { get; set; } = new List<BatterySeries>();

        [Display(Name="مدل یکسوساز")]
        public string RectifierModel { get; set; }

        [Display(Name="ظرفیت یکسوساز")]
        public int EachRectifierCapacity { get; set; }

        [Display(Name="تعداد یکسوساز")]
        public int RectifierCount { get; set; }

        [DisplayName("تعداد سری باتری")]
        public int SeriesCount => Batteries.Count;
    }
}