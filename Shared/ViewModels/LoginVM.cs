using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class LoginVM
    {
        public const int CAPTCHA_CODE_LENGTH = 4;

        [Required(ErrorMessage = "نام کاربری اجباریست!")]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Required(ErrorMessage = "رمز عبور اجباریست!")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا استان خود را انتخاب نمائید.")]
        [Display(Name = "استان")]
        public string Province { get; set; }

        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "تصویر امنیتی اجباریست!")]
        [StringLength(maximumLength: CAPTCHA_CODE_LENGTH, MinimumLength = CAPTCHA_CODE_LENGTH, 
            ErrorMessage = "تعداد کاراکترهای تصویر امنیتی صحیح نیست!")]
        [Display(Name = "تصویر امنیتی")]
        public string Captcha { get; set; }
    }
}
