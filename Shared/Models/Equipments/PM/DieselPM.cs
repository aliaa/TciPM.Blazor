using EasyMongoNet;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "کیفیت شلنگهای آب انتخاب نشده است!")]
        [DisplayName("کیفیت شلنگهای آب")]
        public GoodBad WaterHoseQuality { get; set; } = GoodBad.None;

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "کیفیت شلنگهای گازوئیل انتخاب نشده است!")]
        [DisplayName("کیفیت شلنگهای گازوئیل")]
        public GoodBad GasolineHoseQuality { get; set; } = GoodBad.None;

        [HealthParameter(MinOkRange = 13, MaxOkRange = 28.5)]
        [DisplayName("ولتاژ دینام")]
        public float GeneratorVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 13, MaxOkRange = 28)]
        [DisplayName("ولتاژ تریکل شارژر")]
        public float TrickleChargingVoltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 11.5, MaxOkRange = 13)]
        [DisplayName("ولتاژ باتری استارت 1")]
        public float StartingBattery1Voltage { get; set; } = -1;

        [HealthParameter(MinOkRange = 11.5, MaxOkRange = 13)]
        [DisplayName("ولتاژ باتری استارت 2")]
        public float StartingBattery2Voltage { get; set; } = -1;

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "کیفیت ضدیخ انتخاب نشده است!")]
        [DisplayName("کیفیت ضدیخ")]
        public GoodBad AntiFreezeQuality { get; set; }

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض ضدیخ")]
        public DateTime AntiFreezeChangeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "کیفیت هیتر دیزل انتخاب نشده است!")]
        [DisplayName("کیفیت هیتر دیزل")]
        public GoodBad HeaterQuality { get; set; }

        [HealthParameter(MinOkRange = 45, MaxOkRange = 95)]
        [DisplayName("دما پس از نیم ساعت کار")]
        public int TemperatureAfterHalfHourWork { get; set; } = -1;

        [HealthParameter(MinOkRange = 2, MaxOkRange = 10)]
        [DisplayName("فشار روغن در حال کار")]
        public float OilPressureWhileWorking { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("جریان خروجی [R]")]
        public float OutAmperR { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("جریان خروجی [S]")]
        public float OutAmperS { get; set; } = -1;

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("جریان خروجی [T]")]
        public float OutAmperT { get; set; } = -1;

        [Display(Name = "درصد توان مصرفی")]
        [JsonIgnore]
        public double? PowerConsumptionPercent
        {
            get
            {
                var usage = Math.Max(Math.Max(OutAmperR, OutAmperS), OutAmperT) * 380 * Math.Sqrt(3);
                if (usage <= 0)
                    return null;
                return 100 * usage / (Source.PermissivePower * 1000);
            }
        }

        //[HealthParameter(MinOkRange = 0, MaxOkRange = 80)]
        [DisplayName("حداکثر جریان کشش از دیزل")]
        [JsonIgnore]
        public float MaxAmper => Math.Max(Math.Max(OutAmperR, OutAmperS), OutAmperT);

        [DisplayName("نام کارت کنترل")]
        public string ControlCardName { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "وضعیت کارت کنترل انتخاب نشده است!")]
        [DisplayName("وضعیت کارت کنترل")]
        public GoodBad ControlCardStatus { get; set; }

        public enum OilWaterStatusEnum
        {
            [Display(Name = "")]
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
        [ValidArray(InvalidValues = new string[] { nameof(OilWaterStatusEnum.None) }, ErrorMessage = "سطح آب و روغن انتخاب نشده است!")]
        public OilWaterStatusEnum OilWaterStatus { get; set; }

        [HealthParameter(DateDaysOkRange = 365)]
        [DisplayName("تاریخ تعویض روغن گاورنر")]
        public DateTime GovernerOilChangeDate { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("وضعیت پمپ گازوئیل")]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "وضعیت پمپ گازوئیل انتخاب نشده است!")]
        public GoodBad GasolinePumpStatus { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "کیفیت شلنگ نشاندهنده گازوئیل انتخاب نشده است!")]
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
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "عملکرد کامل دمپرها انتخاب نشده است!")]
        public GoodBad DampersWorkStatus { get; set; }

        [HealthParameter(MinOkRange = 47, MaxOkRange = 53)]
        [DisplayName("فرکانس خروجی")]
        public float OutputFrequency { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("آلارم آب دیزل")]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "آلارم آب دیزل انتخاب نشده است!")]
        public GoodBad WaterAlarm { get; set; }

        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        [DisplayName("آلارم روغن دیزل")]
        [ValidArray(InvalidValues = new string[] { nameof(GoodBad.None) }, ErrorMessage = "آلارم روغن دیزل انتخاب نشده است!")]
        public GoodBad OilAlarm { get; set; }

        [HealthParameter(MinOkRange = 0, MaxOkRange = 3)]
        [DisplayName("ولتاژ بین ارت و نول دیزل")]
        public float VoltageBetweenEarthAndNoleOfDiesel { get; set; } = -1;

        [HealthParameter(MinOkRange = 0, MaxOkRange = 3)]
        [DisplayName("ولتاژ بین ارت و نول برق شهری")]
        public float VoltageBetweenEarthAndNoleOfCity { get; set; } = -1;

        //public Diesel GetSourceObj(IReadOnlyDbContext db)
        //{
        //    if (Source == null)
        //        Source = db.FindById<Diesel>(SourceId);
        //    return (Diesel)Source;
        //}
    

    }
}