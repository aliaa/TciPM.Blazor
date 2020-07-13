using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class AirConditioner : Equipment
    {
        public enum AirConditionerTypeEnum
        {
            [Display(Name="اسپلیت")]
            Split,
            [Display(Name="یک تکه")]
            Solid
        }

        [Display(Name="نوع")]
        public AirConditionerTypeEnum Type { get; set; }



    }
}