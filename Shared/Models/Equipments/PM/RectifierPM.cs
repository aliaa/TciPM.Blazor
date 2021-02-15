using EasyMongoNet;
using System;
using System.ComponentModel;
using System.Linq;
using TciPM.Blazor.Shared.Utils;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    [CollectionIndex(new string[] { nameof(SourceId) })]
    public class RectifierPM : EquipmentPM<RectifierAndBattery>
    {
        public RectifierPM() { }
        public RectifierPM(RectifierAndBattery Source) : base(Source) { }

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 2)]
        [DisplayName("حداکثر جریان DC مصرفی مرکز")]
        public float CenterMaxCurrentUsage { get; set; } = -1;

        //[HealthParameter(MinOkRange = 3600, MaxOkRange = 360000)]
        [DisplayName("مدت زمان دشارژ باتری ها")]
        //[JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan BatteriesDechargeTime { get; set; }

        [HealthParameter(MinOkRange = 53.5, MaxOkRange = 56)]
        [DisplayName("ولتاژ نگهداری")]
        public float SustainVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 54, MaxOkRange = 58.5)]
        [DisplayName("ولتاژ مجدد")]
        public float RepeatingVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 43, MaxOkRange = 51)]
        [DisplayName("ولتاژ نهایی دشارژ")]
        public float FinalDechargeVoltage { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 2)]
        [DisplayName("جریان نهایی دشارژ")]
        public float FinalDechargeCurrent { get; set; } = -1;

        [HealthParameter(MinOkRange = 1.229, MaxOkRange = 1.2601)]
        [DisplayName("حداقل غلظت موجود")]
        public float MinimumDensityExists { get; set; } = -1;

        [HealthParameter(MinOkRange = 1.229, MaxOkRange = 1.2601)]
        [DisplayName("حداکثر غلظت موجود")]
        public float MaximumDensityExists { get; set; } = -1;

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ آخرین دشارژ")]
        public DateTime LastDechargeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("آلارم روی یکسو کننده")]
        public GoodBad Alarm { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("ثبت گذارشات در دفاتر")]
        public GoodBad ReportsAreWriting { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("کارکرد تابلو کنترل و تابلو باتری")]
        public GoodBad ControlAndBatteryEnclosureUsage { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("وضعیت عملکرد آلارم مراکز ریموت مربوط به مرکز مادر")]
        public GoodBad AlarmStatusForRemoteCenters { get; set; }

        [DisplayName("مصرف AC [R]")]
        public float AcUsageR { get; set; }

        [DisplayName("مصرف AC [S]")]
        public float AcUsageS { get; set; }

        [DisplayName("مصرف AC [T]")]
        public float AcUsageT { get; set; }

        //public RectifierAndBattery GetSourceObj(IReadOnlyDbContext db)
        //{
        //    if (Source == null)
        //        Source = db.FindById<RectifierAndBattery>(SourceId);
        //    return (RectifierAndBattery)Source;
        //}

        [DisplayName("درصد جریان مصرفی مرکز به ظرفیت یکسوساز")]
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

        [DisplayName("درصد جریان نهایی دشارژ به ظرفیت باتری")]
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