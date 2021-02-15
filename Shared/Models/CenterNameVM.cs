using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class CenterNameVM
    {
        public string Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "شهر")]
        public string CityName { get; set; }

        [Display(Name = "روزهای گذشته از آخرین PM")]
        public int ElapsedDaysOfLastPm { get; set; }

        public int PMPeriodDays { get; set; }
    }
}
