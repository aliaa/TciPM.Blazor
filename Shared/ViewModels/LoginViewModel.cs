using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری اجباریست!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "رمز عبور اجباریست!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "لطفا استان خود را انتخاب نمائید.")]
        public string Province { get; set; }
        public bool RememberMe { get; set; }
    }
}
