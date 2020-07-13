using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class CommCenterWithReports : CommCenterX
    {
        [Display(Name = "شهر")]
        public string CityName { get; set; }
        [Display(Name = "زمان گذشته از آخرین PM (روز)")]
        public int? ElapsedDaysOfLastPm { get; set; }

        [Display(Name = "تعداد دیزل")]
        public int DieselsCount { get; set; }

        [Display(Name = "تعداد یکسوساز و باتری")]
        public int BatteryAndRectifiersCount { get; set; }

        [Display(Name = "تعداد UPS")]
        public int UpsCount { get; set; }

        [Display(Name = "تعداد گزارشات")]
        public int NotesCount { get; set; }
    }
}
