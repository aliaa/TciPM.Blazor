using MongoDB.Bson;
using System;
using System.Collections.Generic;
using EasyMongoNet;
using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Util;

namespace TciPM.Blazor.Shared.Models
{
    [CollectionIndex(Fields: new string[] { nameof(Date) })]
    [CollectionIndex(Fields: new string[] { nameof(CenterId) })]
    [CollectionIndex(Fields: new string[] { nameof(UserId) })]
    public class DailyPM2 : MongoEntity
    {
        public enum GoodBadDontKnow
        {
            [Display(Name="ندارد")]
            None,
            [Display(Name="بلد نیستم")]
            DontKnow,
            [Display(Name="سالم")]
            Good,
            [Display(Name="ناسالم")]
            Bad,
        }

        [Display(Name="مرکز")]
        public ObjectId CenterId { get; set; }
        [Display(Name="تاریخ")]
        public DateTime Date { get; set; } = DateTime.Now;
        [Display(Name="کاربر")]
        public ObjectId UserId { get; set; }
        [Display(Name="از طرف کارگزار")]
        public bool IsFromCenterWorker { get; set; }

        public GeneralQuestions General { get; set; }
        public SwitchQuestions Switch { get; set; }
        public BuildingQuestions Building { get; set; }
        public DataQuestions Data { get; set; }
        public PowerQuestions Power { get; set; }
        public TransportationQuestions Transportation { get; set; }

        [Display(Name="فهرست بازدیدکنندگان از مرکز در آن روز")]
        public List<Visitor> Visitors { get; set; }

        public Dictionary<ObjectId, GoodBad> AirConditionersStatus { get; set; } = new Dictionary<ObjectId, GoodBad>();

        public class GeneralQuestions
        {
            [Display(Name="وضعیت کامپیوتر سوئیچ شهری")]
            public GoodBad PcStatus { get; set; }

            [Display(Name="وضعیت سیستم گرمایشی")]
            public GoodBad HeatingSystemStatus { get; set; }

            [Display(Name="وضعیت مسدودی راههای نفوذ آب به سالن دستگاه")]
            public GoodBad WaterPenetrationStatus { get; set; }

            [Display(Name="دمای سالن دستگاه")]
            [HealthParameter(MinOkRange = 10, MaxOkRange = 30)]
            public float SaloonTemperature { get; set; }

            [Display(Name="روشنایی سالن ها")]
            public GoodBad SaloonsBrightness { get; set; }

            [Display(Name="سرویس آبدار خانه")]
            public GoodBad PantryStatus { get; set; }

            [Display(Name="رنگ آمیزی دیوار و سقف، کفسازی و درزگیری")]
            public GoodBad ColoringStatus { get; set; }

            [Display(Name="نمای ساختمان و محوطه")]
            public GoodBad BuildingFacadeStatus { get; set; }

            [Display(Name="سیستم لوله کشی سرد و گرم")]
            public GoodBad PiplineStatus { get; set; }

            [Display(Name="نظافت سیستم انتقال، شلف ها و کابل کشی ها")]
            public GoodBad TransportationCleaningStatus { get; set; }

            [Display(Name="دستگاه اطفاء حريق")]
            public GoodBad FireEquipment { get; set; }

            [Display(Name="تاریخ شارژ دستگاه اطفاء حریق")]
            public DateTime FireEquipmentChargeDate { get; set; }

            [Display(Name="وسایل اضافی در سالن دارد")]
            public bool AdditionalItemsInSaloon { get; set; }

            [Display(Name="تعداد باتری خراب")]
            public int BrokenBatteriesCount { get; set; }

            [Display(Name="دمای بدنه باتری")]
            public GoodBad BatteryTemperatureStatus { get; set; }

            [Display(Name="وضعیت نشتی اسید باتریها")]
            public GoodBad BatteriesAcidLeakStatus { get; set; }

            [Display(Name="توضیح")]
            public string Description { get; set; }
        }

        public class SwitchQuestions
        {

            [Display(Name="وضعیت عمومی مرکز سوئیچ شهری")]
            public GoodBad GeneralSwitchStatus { get; set; }

            [Display(Name="تست بوق آزاد مشترکین")]
            public GoodBad CustomersFreeBeepTest { get; set; }

            [Display(Name="تست ارتباط با مراکز دیگر")]
            public GoodBad ConnectionToOtherCentersTest { get; set; }

            [Display(Name="تست Caller ID مشترکین")]
            public GoodBad CustomersCallerIdTest { get; set; }

            [Display(Name="ارتباط با هات بیلینگ")]
            public GoodBadDontKnow HotBillingConnection { get; set; }
            
            [Display(Name="تعداد پورتهای خراب (خرابی مشترکین)")]
            public int BrokenPortsCount { get; set; }

            [Display(Name="تعداد کارتهای یدکی سالم سوئیچ شهری")]
            public int HealthyBackupSwitchCardsCount { get; set; }

            [Display(Name="تعداد کارتهای یدکی معیوب سوئیچ شهری")]
            public int BrokenBackupSwitchCardsCount { get; set; }

            [Display(Name="تعداد کارتهای یدکی معیوب سوئیچ شهری ارسال شده به تعمیرات")]
            public int BrokenBackupSwitchSentToRepairCount { get; set; }

            [Display(Name="تست بوق آزاد در مرکز با DLC زیرمجموعه")]
            public GoodBadDontKnow DlcFreeBeepTest { get; set; }

            [Display(Name="وضعیت عمومی  DLC موجود در مرکز")]
            public GoodBadDontKnow DlcGeneralStatus { get; set; }

            [Display(Name="تعداد کارتهای یدکی سالم DLC")]
            public int HealthyBackupDlcCardsCount { get; set; }

            [Display(Name="تعداد کارتهای یدکی معیوب DLC")]
            public int BrokenBackupDlcCardsCount { get; set; }

            [Display(Name="تعداد کارتهای یدکی معیوب DLC ارسال شده به تعمیرات")]
            public int BrokenBackupDlcCardsCountSentToRepair { get; set; }
        }

        public class BuildingQuestions
        {
            public enum EarthingSystemStatusEnum
            {
                [Display(Name="بلد نیستم")]
                DontKnow,
                [Display(Name="نرمال")]
                Normal,
                [Display(Name="قطع بر اثر خوردگی")]
                CutByErosion,
                [Display(Name="قطع بر اثر سرقت")]
                CutByStealing
            }

            [Display(Name="وضعیت سیم کشی ساختمان و خرابیهاو خرابی های برق")]
            public GoodBad ElectricityStatus { get; set; }

            [Display(Name="ایزوگام پشت بام")]
            public GoodBad RoofTopInsulation { get; set; }

            [Display(Name="سیستم لوله کشی فاضلاب")]
            public GoodBad SanitationPipline { get; set; }

            [Display(Name="سیستم لوله کشی آب باران")]
            public GoodBad RainPipline { get; set; }

            [Display(Name="وضعیت شینه ارت")]
            [HealthParameter(EnumOkItems = new string[] { nameof(EarthingSystemStatusEnum.CutByErosion), nameof(EarthingSystemStatusEnum.CutByStealing) })]
            public EarthingSystemStatusEnum EarthingSystemStatus { get; set; }

            [Display(Name="وضعیت تابلو برق")]
            public GoodBad ElectricityPanelStatus { get; set; }
        }

        public class DataQuestions
        {
            public enum DataEquipmentStatusEnum
            {
                [Display(Name="موجود نیست")]
                NotExists,
                [Display(Name="در حال کار")]
                Working,
                [Display(Name="خاموش")]
                ShutDown,
                [Display(Name="مشکل دار")]
                HasProblems,
                [Display(Name="بلد نیستم")]
                DontKnow,
            }

            [Display(Name="وضعیت ترانک (اترنت یا 2 مگا)")]
            public GoodBad TrunkStatus { get; set; }

            [Display(Name="وضعیت تجهیزات Dslam")]
            [HealthParameter(EnumOkItems = new string[] { nameof(DataEquipmentStatusEnum.Working) })]
            public DataEquipmentStatusEnum DslamStatus { get; set; }

            [Display(Name="وضعیت تجهیزات Tellabs")]
            [HealthParameter(EnumOkItems = new string[] { nameof(DataEquipmentStatusEnum.Working) })]
            public DataEquipmentStatusEnum TellabsStatus { get; set; }

            [Display(Name="وضعیت تجهیزات Data Switch")]
            [HealthParameter(EnumOkItems = new string[] { nameof(DataEquipmentStatusEnum.Working) })]
            public DataEquipmentStatusEnum DataSwitchStatus { get; set; }
        }

        public class PowerQuestions
        {
            [Display(Name="تعداد ماژول غیر نرمال یکسوکننده")]
            [HealthParameter(MaxOkRange = 0)]
            public int UnNormalRectifierModuleCount { get; set; }

            [Display(Name="ولتاژ یکسو کننده")]
            [HealthParameter(MinOkRange = 53, MaxOkRange = 56.5)]
            public float RectifierVoltage { get; set; }

            [Display(Name="جریان یکسو کننده")]
            public float RectifierCurrent { get; set; }

            [Display(Name="کلید باتری در یکسوساز وصل است")]
            public bool BatterySwitchIsOn { get; set; }

            [Display(Name="وضعیت سطح اسید باتری ها")]
            public GoodBad BatteriesAcidAmountStatus { get; set; }

            [Display(Name="وضعیت تورم باتری ها")]
            public GoodBad BatteriesBlownStatus { get; set; }
        }

        public class TransportationQuestions
        {
            [Display(Name="تست Order Wire")]
            public GoodBad OrderWireTest { get; set; }

            [Display(Name="تست آلارم شنيداري بالای راک ")]
            public GoodBad RackAlarmTest { get; set; }

            [Display(Name="سيركولاتور هواي داخل راك")]
            public GoodBad RackAirCirculatorStatus { get; set; }

            [Display(Name="وضعيت آلارم دستگاهها (LED)")]
            public GoodBad EquipmentsLedAlarm { get; set; }

            [Display(Name="وضعیت سوراخ فیدر")]
            public GoodBad FeederHoleStatus { get; set; }
        }

        public class Visitor
        {
            [Display(Name="نام و نام خانوادگی")]
            public string Name { get; set; }
            [Display(Name="دلیل بازدید")]
            public string CauseOfVisit { get; set; }
            [Display(Name="عملیات انجام شده")]
            public string WorkedOperations { get; set; }
        }
    }

}