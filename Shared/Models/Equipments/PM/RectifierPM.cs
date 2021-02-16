using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            [Display(Name = "")]
            None,
            [Display(Name = "عدم رسیدن موعد دشارژ")]
            DechargeTimeNotReached,
            [Display(Name = "غیر قابل اعتماد بودن باتری ها")]
            BatteriesAreNotTrusted,
            [Display(Name = "سایر")]
            Others,
        }

        [Display(Name = "عملیات PM باتریها با خاموش کردن یکسوکننده انجام گرفت")]
        public bool RectifierShutdownInPm { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [ValidArray(InvalidValues = new string[] { nameof(RectifierNotShuttingDownReason.None) }, ErrorMessage = "دلیل عدم خاموش کردن یکسوساز انتخاب نشده است!")]
        [Display(Name ="دلیل عدم خاموش کردن")]
        public RectifierNotShuttingDownReason ShutDownReason { get; set; }

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 2)]
        [Display(Name = "حداکثر جریان DC مصرفی مرکز")]
        public float CenterMaxCurrentUsage { get; set; } = -1;

        //[HealthParameter(MinOkRange = 3600, MaxOkRange = 360000)]
        [Display(Name = "مدت زمان دشارژ باتری ها")]
        //[JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan BatteriesDechargeTime { get; set; }

        [HealthParameter(MinOkRange = 53.5, MaxOkRange = 56)]
        [Display(Name = "ولتاژ نگهداری")]
        public float SustainVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 54, MaxOkRange = 58.5)]
        [Display(Name = "ولتاژ مجدد")]
        public float RepeatingVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 43, MaxOkRange = 51)]
        [Display(Name = "ولتاژ نهایی دشارژ")]
        public float FinalDechargeVoltage { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 2)]
        [Display(Name = "جریان نهایی دشارژ")]
        public float FinalDechargeCurrent { get; set; } = -1;

        [HealthParameter(MinOkRange = 1.229, MaxOkRange = 1.2601)]
        [Display(Name = "حداقل غلظت موجود")]
        public float MinimumDensityExists { get; set; } = -1;

        [HealthParameter(MinOkRange = 1.229, MaxOkRange = 1.2601)]
        [Display(Name = "حداکثر غلظت موجود")]
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
        public float AcUsageR { get; set; }

        [Display(Name = "مصرف AC [S]")]
        public float AcUsageS { get; set; }

        [Display(Name = "مصرف AC [T]")]
        public float AcUsageT { get; set; }

        //public RectifierAndBattery GetSourceObj(IReadOnlyDbContext db)
        //{
        //    if (Source == null)
        //        Source = db.FindById<RectifierAndBattery>(SourceId);
        //    return (RectifierAndBattery)Source;
        //}

        [Display(Name = "درصد جریان مصرفی مرکز به ظرفیت یکسوساز")]
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