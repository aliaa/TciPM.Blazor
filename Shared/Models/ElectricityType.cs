using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public enum ElectricityType
    {
        [Display(Name="تک فاز")]
        SinglePhase,

        [Display(Name="سه فاز")]
        ThreePhase
    }
}