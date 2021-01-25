using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    [CollectionSave(WriteLog = true)]
    public class Diesel : Equipment
    {
        [Display(Name="قدرت نامی")]
        public int Power { get; set; }

        [Display(Name="ظرفیت Day Tank")]
        public int StorageCapacity { get; set; }

        [Display(Name="ظرفیت مخزن اصلی")]
        public int MainStorageCapacity { get; set; }

        [Display(Name="ظرفیت روغن")]
        public int OilCapacity { get; set; }

        [Display(Name="ظرفیت رادیاتور")]
        public int RadiatorCapacity { get; set; }

        [Display(Name="ضریب راندمان")]
        [Range(minimum:0.1, maximum: 1, ErrorMessage = "ضریب راندمان بایستی بین 0.1 تا 1 باشد.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Double, AllowTruncation = true)]
        public float EfficiencyPercentage { get; set; } = 0.8f;

        [Display(Name="ضریب اختلاف از سطح دریا")]
        [Range(minimum: 0.1, maximum: 1, ErrorMessage = "ضریب اختلاف از سطح دریا بایستی بین 0.1 تا 1 باشد.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Double, AllowTruncation = true)]
        public float AltitudePercentage { get; set; } = 0.9f;

        [Display(Name="ضریب راه اندازی")]
        [Range(minimum: 0.1, maximum: 1, ErrorMessage = "ضریب راه اندازی بایستی بین 0.1 تا 1 باشد.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Double, AllowTruncation = true)]
        public float InitiationPercentage { get; set; } = 0.9f;

        [Display(Name="قدرت مجاز")]
        public int PermissivePower => (int)Math.Round(Power * EfficiencyPercentage * AltitudePercentage * InitiationPercentage);

        [Display(Name="جریان مجاز")]
        public int PermissiveCurrent => (int)Math.Round(PermissivePower * 1000 / (Math.Sqrt(3) * 380));

        [Display(Name="تعداد باتری دیزل")]
        [Range(minimum: 1, maximum: 2, ErrorMessage = "تعداد باتری بایستی 1 یا 2 باشد!")]
        public int DieselBatteryCount { get; set; } = 2;
    }
}