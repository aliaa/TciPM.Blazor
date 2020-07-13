using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using TciCommon.Models;

namespace TciPM.Blazor.Shared.Models
{
    [CollectionIndex(new string[] { nameof(City) })]
    [CollectionIndex(new string[] { nameof(Name) })]
    [CollectionOptions(Name = "CommCenter")]
    [CollectionSave(WriteLog = true)]
    public class CommCenterX : CommCenter
    {
        public enum SwitchTypeEnum
        {
            EWSD, Carin, FiberHome, Guyan, Kiatel, Neax, Parstel, ZTE, Kafu, Remote, Huawei, Keymile, OAX, Digitron, WLL, BTS, Siemens_S12,
            [Display(Name = "کارا")]
            Kara,
            [Display(Name = "کامکار")]
            Kamkar,
            [Display(Name = "سایت رادیوئی")]
            RadioSite,
            [Display(Name = "ندارد")]
            None,
        }

        public const int DEFAULT_PM_PERIOD_DAYS = 30;

        public int PMPeriodDays { get; set; } = DEFAULT_PM_PERIOD_DAYS;

        [Display(Name = "PM تجهیزات فعال")]
        public bool EquipmentsPmEnabled { get; set; } = true;

        [Display(Name = "PM روزانه فعال")]
        public bool DailyPmEnabled { get; set; } = true;

        [Display(Name = "ظرفیت مرکز")]
        public int CenterCapacity { get; set; }

        [Display(Name = "نوع سوئیچ")]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public SwitchTypeEnum SwitchType { get; set; }

        [Display(Name = "درجه اهمیت")]
        public int ImportanceLevel { get; set; } = 1;
    }
}
