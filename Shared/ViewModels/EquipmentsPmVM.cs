using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Models;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class EquipmentsPmVM : EquipmentsPM
    {
        [Display(Name = "شهر")]
        public string CityName { get; set; }

        [Display(Name = "مرکز")]
        public string CenterName { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public string ReportingUserName { get; set; }
    }
}
