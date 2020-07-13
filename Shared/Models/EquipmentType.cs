using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public enum EquipmentType
    {
        [Display(Name = "دیزل ژنراتور")]
        Diesel,
        [Display(Name = "یکسو کننده")]
        Rectifier,
        [Display(Name = "باتری")]
        Battery,
        [Display(Name = "UPS")]
        UPS,
    }
}
