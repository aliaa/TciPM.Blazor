using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class EquipmentsPmListItemVM
    {
        public string Id { get; set; }

        [Display(Name = "شهر")]
        public string City { get; set; }
        
        [Display(Name = "مرکز")]
        public string Center { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public string SubmitDate { get; set; }

        [Display(Name = "تاریخ انجام PM")]
        public string PmDate { get; set; }

        [Display(Name = "تاریخ تغییر")]
        public string EditDate { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public string ReportingUser { get; set; }
        
        [Display(Name = "امتیاز کلی مرکز")]
        public int TotalRate { get; set; }
    }
}
