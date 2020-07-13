using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class CapacitorBank : AcEquipment
    {
        public enum CapacitorType
        {
            [Display(Name="خشک")]
            Dry,
            [Display(Name="روغنی")]
            Oily
        }

        public enum ControlType
        {
            [Display(Name="اتوماتیک")]
            Automatic,
            [Display(Name="ثابت")]
            Fixed,
        }

        public class Step
        {
            [DisplayName("نوع خازن")]
            public CapacitorType CapacitorType { get; set; }

            [DisplayName("نوع کنترل")]
            public ControlType ControlType { get; set; }

            [DisplayName("ظرفیت خازن")]
            public float Capacity { get; set; }
        }

        public List<Step> Steps { get; set; } = new List<Step>();
    }
}