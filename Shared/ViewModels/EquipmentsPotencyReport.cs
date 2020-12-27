using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class EquipmentsPotencyReport
    {
        public enum ReportType
        {
            [Display(Name = "درصد توان مصرفی دیزل به قدرت مجاز")]
            DieselPowerConsumptionPercent,
            [Display(Name = "درصد جریان مصرفی مرکز به ظرفیت یکسوساز")]
            RectifierPowerConsumptionPercent,
            [Display(Name = "درصد جریان نهایی دشارژ به ظرفیت باتری")]
            RectifierFinalDechargePercent,
            [Display(Name = "انحراف معیار جریان خروجی دیزل")]
            DieselOutAmperDeviation,
            [Display(Name = "انحراف معیار ولتاژ برق شهر")]
            CityVoltageDeviation,
            [Display(Name = "انحراف معیار ولتاژ خروجی دیزل")]
            DieselOutVoltageDeviation,
            [Display(Name = "انحراف معیار جریان مصرف یکسوکننده ها")]
            RectifierAmperDeviation,
            [Display(Name = "راندمان یکسوساز")]
            RectifierPerformance,
        }

        public enum ReportSource
        {
            [Display(Name = "آخرین PM ها")]
            LastPMs,
            [Display(Name = "مرکز مشخص")]
            SpecificCenter
        }

        public enum LessGreater
        {
            [Display(Name = "همه")]
            All,
            [Display(Name = "بزرگتر از")]
            GreaterThan,
            [Display(Name = "کوچکتر از")]
            LessThan,
        }

        [Display(Name = "نوع گزارش")]
        public ReportType Type { get; set; }

        [Display(Name = "منبع گزارش")]
        public ReportSource SourceType { get; set; }

        public LessGreater CenterCapacityLessGreater { get; set; }

        public int CenterCapacityThreshold { get; set; }

        [Display(Name = "درجه اهمیت مرکز بیشتر یا برابر")]
        public int CenterImportance { get; set; } = 1;

        [Display(Name = "مرکز")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Center { get; set; }
    }
}
