using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    public class GasCablePM : EquipmentPM<GasCable>
    {
        public GasCablePM() { }

        public GasCablePM(GasCable Source) : base(Source) { }

        [Display(Name = "فشار ابتدا")]
        public int StartPressure { get; set; }

        [Display(Name = "فشار مفصل")]
        public int? BranchPressure { get; set; }

        [Display(Name = "فشار مقصدها")]
        public List<int> DestinationsPressure { get; set; } = new List<int>();
    }
}
