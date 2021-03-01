using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using TciPM.Blazor.Shared.Utils;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    [CollectionIndex(new string[] { nameof(SourceId) })]
    public class RectifierPM : EquipmentPM<RectifierAndBattery>
    {
        public RectifierPM() { }
        public RectifierPM(RectifierAndBattery Source) : base(Source) { }

        public enum RectifierNotShuttingDownReason
        {
            [Display(Name = "عدم رسیدن موعد دشارژ")]
            DechargeTimeNotReached,
            [Display(Name = "غیر قابل اعتماد بودن باتری ها")]
            BatteriesAreNotTrusted,
            [Display(Name = "سایر")]
            Others,
        }

        [Display(Name = "عملیات PM باتریها با خاموش کردن یکسوکننده انجام گرفت")]
        public bool RectifierShutdownInPm { get; set; } = true;

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [Display(Name ="دلیل عدم خاموش کردن")]
        public RectifierNotShuttingDownReason ShutDownReason { get; set; }

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 2)]
        [Display(Name = "حداکثر جریان DC مصرفی مرکز")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "حداکثر جریان DC مصرفی مرکز اجباری است!")]
        public float CenterMaxCurrentUsage { get; set; } = -1;

        //[HealthParameter(MinOkRange = 3600, MaxOkRange = 360000)]
        [Display(Name = "مدت زمان دشارژ باتری ها")]
        //[JsonConverter(typeof(TimeSpanJsonConverter))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "مدت زمان دشارژ باتری ها اجباری است!")]
        public TimeSpan BatteriesDechargeTime { get; set; }

        [HealthParameter(MinOkRange = 53.5, MaxOkRange = 56)]
        [Display(Name = "ولتاژ نگهداری")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "ولتاژ نگهداری اجباری است!")]
        public float SustainVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 54, MaxOkRange = 58.5)]
        [Display(Name = "ولتاژ مجدد")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "ولتاژ مجدد اجباری است!")]
        public float RepeatingVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 43, MaxOkRange = 51)]
        [Display(Name = "ولتاژ نهایی دشارژ")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "ولتاژ نهایی دشارژ اجباری است!")]
        public float FinalDechargeVoltage { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 2)]
        [Display(Name = "جریان نهایی دشارژ")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "جریان نهایی دشارژ اجباری است!")]
        public float FinalDechargeCurrent { get; set; } = -1;

        [HealthParameter(MinOkRange = 1.229, MaxOkRange = 1.2601)]
        [Display(Name = "حداقل غلظت موجود")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "حداقل غلظت موجود اجباری است!")]
        public float MinimumDensityExists { get; set; } = -1;

        [HealthParameter(MinOkRange = 1.229, MaxOkRange = 1.2601)]
        [Display(Name = "حداکثر غلظت موجود")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "حداکثر غلظت موجود اجباری است!")]
        public float MaximumDensityExists { get; set; } = -1;

        [HealthParameter(DateDaysOkRange = 365)]
        [Display(Name = "تاریخ آخرین دشارژ")]
        public DateTime LastDechargeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [Display(Name = "آلارم روی یکسو کننده")]
        public GoodBad Alarm { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [Display(Name = "ثبت گذارشات در دفاتر")]
        public GoodBad ReportsAreWriting { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [Display(Name = "کارکرد تابلو کنترل و تابلو باتری")]
        public GoodBad ControlAndBatteryEnclosureUsage { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [Display(Name = "وضعیت عملکرد آلارم مراکز ریموت مربوط به مرکز مادر")]
        public GoodBad AlarmStatusForRemoteCenters { get; set; }

        [Display(Name = "مصرف AC [R]")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "مصرف AC [R] اجباری است!")]
        public float AcUsageR { get; set; } = -1;

        [Display(Name = "مصرف AC [S]")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "مصرف AC [S] اجباری است!")]
        public float AcUsageS { get; set; } = -1;

        [Display(Name = "مصرف AC [T]")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "مصرف AC [T] اجباری است!")]
        public float AcUsageT { get; set; } = -1;

        //public RectifierAndBattery GetSourceObj(IReadOnlyDbContext db)
        //{
        //    if (Source == null)
        //        Source = db.FindById<RectifierAndBattery>(SourceId);
        //    return (RectifierAndBattery)Source;
        //}

        [Display(Name = "درصد جریان مصرفی مرکز به ظرفیت یکسوساز")]
        [JsonIgnore]
        public float? PowerConsumptionPercent
        {
            get
            {
                int totalCapacity = Source.RectifierCount * Source.EachRectifierCapacity;
                var result = 100 * CenterMaxCurrentUsage / totalCapacity;
                if (result < 0)
                    return null;
                return result;
            }
        }

        [Display(Name = "درصد جریان نهایی دشارژ به ظرفیت باتری")]
        [JsonIgnore]
        public float FinalDechargePercent
        {
            get
            {
                var capacitySum = Source.Batteries.Sum(s => s.Capacity);
                if (capacitySum == 0)
                {
                    capacitySum = Source.Batteries.Sum(s => s.Capacity);
                }
                return 100 * FinalDechargeCurrent / capacitySum;
            }
        }
    }
}