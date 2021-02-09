using EasyMongoNet;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Utils;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    [CollectionIndex(new string[] { nameof(SourceId) })]
    public class DieselPM : EquipmentPM<Diesel>
    {
        public DieselPM() { }
        public DieselPM(Diesel Source) : base(Source) { }

        [DisplayName("ساعت کارکرد فعلی")]
        public int CurrentUsageTime { get; set; } = -1;

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض فیلتر گازوئیل")]
        public DateTime GasolineFilterChangeDate { get; set; }

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض روغن")]
        public DateTime OilChangeDate { get; set; }

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض فیلتر روغن")]
        public DateTime OilFilterChangeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("کیفیت شلنگهای آب")]
        public GoodBad WaterHoseQuality { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("کیفیت شلنگهای گازوئیل")]
        public GoodBad GasolineHoseQuality { get; set; }

        [HealthParameter(MinOkRange = 11.5, MaxOkRange = 13)]
        [DisplayName("ولتاژ باتری استارت 1")]
        public float StartingBattery1Voltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 11.5, MaxOkRange = 13)]
        [DisplayName("ولتاژ باتری استارت 2")]
        public float StartingBattery2Voltage { get; set; } = -1;

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("کیفیت ضدیخ")]
        public GoodBad AntiFreezeQuality { get; set; }

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض ضدیخ")]
        public DateTime AntiFreezeChangeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("کیفیت هیتر دیزل")]
        public GoodBad HeaterQuality { get; set; }

        [HealthParameter(MinOkRange = 45, MaxOkRange = 95)]
        [DisplayName("دمای دیزل پس از نیم ساعت کار")]
        public int TemperatureAfterHalfHourWork { get; set; } = -1;

        [HealthParameter(MinOkRange = 2, MaxOkRange = 10)]
        [DisplayName("فشار روغن در حال کار")]
        public float OilPressureWhileWorking { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("حداکثر جریان کشش از دیزل")]
        public float MaxAmper => Math.Max(Math.Max(OutAmperR, OutAmperS), OutAmperT);

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("جریان خروجی دیزل [R]")]
        public float OutAmperR { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("جریان خروجی دیزل [S]")]
        public float OutAmperS { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("جریان خروجی دیزل [T]")]
        public float OutAmperT { get; set; } = -1;

        [DisplayName("نام کارت کنترل")]
        public string ControlCardName { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("وضعیت کارت کنترل")]
        public GoodBad ControlCardStatus { get; set; }

        public enum OilWaterStatusEnum
        {
            None,
            [Display(Name="کم")]
            Low,
            [Display(Name="زیاد")]
            High,
            [Display(Name="مخلوط آب و روغن")]
            Mixed,
            [Display(Name="نرمال")]
            Normal,
        }

        [HealthParameter(EnumOkItems = new string[] { nameof(OilWaterStatusEnum.Normal) })]
        [DisplayName("سطح آب و روغن")]
        public OilWaterStatusEnum OilWaterStatus { get; set; }

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض روغن گاورنر")]
        public DateTime GovernerOilChangeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("وضعیت پمپ گازوئیل")]
        public GoodBad GasolinePumpStatus { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("کیفیت شلنگ نشاندهنده گازوئیل")]
        public GoodBad GasolineViewHoseQuality { get; set; }

        [HealthParameter(MinOkRange = 187, MaxOkRange = 242)]
        [DisplayName("ولتاژ برق شهر [R-N]")]
        public float CityVoltageRN { get; set; } = -1;

        [HealthParameter(MinOkRange = 187, MaxOkRange = 242)]
        [DisplayName("ولتاژ برق شهر [S-N]")]
        public float CityVoltageSN { get; set; } = -1;

        [HealthParameter(MinOkRange = 180, MaxOkRange = 240)]
        [DisplayName("ولتاژ برق شهر [T-N]")]
        public float CityVoltageTN { get; set; } = -1;

        [HealthParameter(MinOkRange = 187, MaxOkRange = 242)]
        [DisplayName("ولتاژ خروجی دیزل [R-N]")]
        public float DieselVoltageRN { get; set; } = -1;

        [HealthParameter(MinOkRange = 187, MaxOkRange = 242)]
        [DisplayName("ولتاژ خروجی دیزل [S-N]")]
        public float DieselVoltageSN { get; set; } = -1;

        [HealthParameter(MinOkRange = 187, MaxOkRange = 242)]
        [DisplayName("ولتاژ خروجی دیزل [T-N]")]
        public float DieselVoltageTN { get; set; } = -1;

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("عملکرد کامل دمپرها")]
        public GoodBad DampersWorkStatus { get; set; }

        [HealthParameter(MinOkRange = 13, MaxOkRange = 28.5)]
        [DisplayName("ولتاژ دینام")]
        public float GeneratorVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 13, MaxOkRange = 28)]
        [DisplayName("ولتاژ تریکل شارژر")]
        public float TrickleChargingVoltage { get; set; } = -1;

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("آلارم آب دیزل")]
        public GoodBad WaterAlarm { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("آلارم روغن دیزل")]
        public GoodBad OilAlarm { get; set; }

        [HealthParameter(MinOkRange = 0, MaxOkRange = 3)]
        [DisplayName("ولتاژ بین ارت و نول دیزل")]
        public float VoltageBetweenEarthAndNoleOfDiesel { get; set; } = -1;

        [HealthParameter(MinOkRange = 0, MaxOkRange = 3)]
        [DisplayName("ولتاژ بین ارت و نول برق شهری")]
        public float VoltageBetweenEarthAndNoleOfCity { get; set; } = -1;

        [HealthParameter(MinOkRange = 47, MaxOkRange = 53)]
        [DisplayName("فرکانس خروجی")]
        public float OutputFrequency { get; set; }

        //public Diesel GetSourceObj(IReadOnlyDbContext db)
        //{
        //    if (Source == null)
        //        Source = db.FindById<Diesel>(SourceId);
        //    return (Diesel)Source;
        //}
    

        private const int V = 380;
        private static readonly float RADICAL3 = (float)Math.Sqrt(3);

        [DisplayName("درصد توان مصرفی دیزل به قدرت مجاز")]
        public float GetPowerConsumptionPercent(Diesel diesel)
        {
            float usagePower = Math.Max(Math.Max(OutAmperR, OutAmperS), OutAmperT) * V * RADICAL3;
            return 100 * usagePower / (diesel.PermissivePower * 1000);
        }
    }
}