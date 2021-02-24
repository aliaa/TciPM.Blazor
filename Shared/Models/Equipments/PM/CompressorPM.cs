using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    public class CompressorPM : EquipmentPM<Compressor>
    {
        public CompressorPM() { }

        public CompressorPM(Compressor Source) : base(Source) { }

        [Display(Name = "مدت زمان کارکرد (دقیقه)")]
        public int Counter { get; set; }

        [Display(Name = "دارای آلارم رطوبت")]
        public bool HasMoistureAlarm { get; set; }

        [Display(Name = "دارای آلارم Time Out")]
        public bool HasTimeOutAlarm { get; set; }

        [Display(Name = "دارای آلارم فشار بالا")]
        public bool HasHighPressureAlarm { get; set; }

        [Display(Name = "دارای آلارم فشار پایین")]
        public bool HasLowPressureAlarm { get; set; }
    }
}
