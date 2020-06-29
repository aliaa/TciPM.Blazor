using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared
{
    public enum Permission
    {
        [Display(Name = "ثبت و تغییر PM تجهیزات")]
        WriteEquipmentPM,
        [Display(Name = "مدیریت کاربران")]
        ManageUsers,
        [Display(Name = "تغییر مراکز")]
        ChangeCenters,
        [Display(Name = "تغییر شهرها")]
        ChangeCities,
        [Display(Name = "گزارش گیری")]
        ViewReports,
        [Display(Name = "حذف تجهیزات")]
        DeleteEquipments,
        [Display(Name = "ثبت PM روزانه")]
        WriteDailyPM,
        [Display(Name = "مشاهده PM ها")]
        ShowPMs,
        [Display(Name = "نمایش اطلاعات مراکز")]
        ShowCenters,
        [Display(Name = "طراحی تجهیزات AC")]
        AcSystemDesign,
        [Display(Name = "PM تجهیزات AC")]
        AcSystemPM
    }
}
