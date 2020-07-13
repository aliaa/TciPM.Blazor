using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Util
{
    public class EquipmentProblem
    {
        [Display(Name="شهر")]
        public string City { get; set; }

        [Display(Name="مرکز")]
        public string Center { get; set; }

        [Display(Name="نوع دستگاه")]
        public EquipmentType DeviceType { get; set; }

        public ObjectId DeviceId { get; set; }

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