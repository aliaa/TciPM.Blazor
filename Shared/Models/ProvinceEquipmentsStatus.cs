using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class ProvinceEquipmentsStatus
    {
        [Display(Name = "استان")]
        public string ProvinceName { get; set; }

        [Display(Name = "تعداد دیزل")]
        public int DieselCount { get; set; }

        [Display(Name = "جمع قدرت نامی دیزلها")]
        public int DieselPowerSum { get; set; }

        [Display(Name = "جمع قدرت مجاز دیزلها")]
        public int PermissivePowerSum { get; set; }

        [Display(Name = "تعداد یکسوساز و باتری")]
        public int RectifierCount { get; set; }

        [Display(Name = "جمع ظرفیت یکسوسازها")]
        public int RectifiersCapacitySum { get; set; }

        [Display(Name = "تعداد کل سریهای باتری")]
        public int BatterySeriesCount { get; set; }

        [Display(Name = "جمع ظرفیت سریهای باتری")]
        public int BatterySeriesCapacitySum { get; set; }
    }
}
