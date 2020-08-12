using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class ProvinceStatus : CentersAggregatedData
    {
        [Display(Name = "تعداد کاربر")]
        public int UserCount { get; set; }
    }
}
