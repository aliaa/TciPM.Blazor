using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Models;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class EquipmentProblem
    {
        [Display(Name="شهر")]
        public string City { get; set; }

        [Display(Name="مرکز")]
        public string Center { get; set; }

        [Display(Name="نوع دستگاه")]
        public EquipmentType DeviceType { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [Display(Name="نام دستگاه")]
        public string DeviceName { get; set; }

        [Display(Name="نام ویژگی")]
        public string PropertyName { get; set; }

        [Display(Name="آخرین مقدار")]
        public string LastValue { get; set; }

        [Display(Name="روزهای حل نشده")]
        public int DaysNotRepaired { get; set; }
    }
}