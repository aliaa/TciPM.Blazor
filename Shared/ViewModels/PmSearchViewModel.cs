using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class PmSearchViewModel
    {
        [Display(Name = "شهر")]
        public string City { get; set; }

        [Display(Name = "مرکز")]
        public string Center { get; set; }

        [Display(Name = "از تاریخ")]
        public string FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        public string ToDate { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public string SubmittedUser { get; set; }
    }
}
