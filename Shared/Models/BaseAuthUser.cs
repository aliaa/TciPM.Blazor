using MongoDB.Bson;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public abstract class BaseAuthUser
    {
        public ObjectId Id { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name="مدیر سیستم است؟")]
        public bool IsAdmin { get; set; }

        [Required]
        [Display(Name="نام")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name="غیر فعال شده")]
        public bool Disabled { get; set; }

        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }

        public List<Permission> Permissions { get; set; } = new List<Permission>();
        public List<ObjectId> Cities { get; set; } = new List<ObjectId>();
        
        [Display(Name = "IP محدود شده")]
        public string RestrictedIP { get; set; }

        [Display(Name = "کارگزار مراکز کم ظرفیت است؟")]
        public bool IsDailyCenterWorker { get; set; }

        public List<ObjectId> AllowedDailyCenters { get; set; } = new List<ObjectId>();
        public List<EquipmentType> AllowedEquipmentTypes { get; set; } = new List<EquipmentType>();
        public bool IsSuperAdmin { get; set; }

        public bool HasPermission(Permission perm)
        {
            return IsAdmin || Permissions.Contains(perm);
        }
    }
}
