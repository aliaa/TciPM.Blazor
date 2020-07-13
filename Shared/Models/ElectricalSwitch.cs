using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class ElectricalSwitch
    {
        public enum SwitchType
        {
            [Display(Name="اتوماتیک")]
            Automatic,
            [Display(Name="مینیاتوری تکفاز")]
            Miniature1Phase,
            [Display(Name="مینیاتوری 3 فاز")]
            Miniature3Phase,
            [Display(Name="کلید فیوز")]
            Fuse,
            [Display(Name="زاویر")]
            Zavir,
            [Display(Name="گردان")]
            Rounding,
        }

        [DisplayName("نوع")]
        public SwitchType Type { get; set; }

        [DisplayName("جریان نامی")]
        public float NamingCurrent { get; set; }

        [DisplayName("جریان تنظیمی")]
        public float SettingCurrent { get; set; }
    }
}