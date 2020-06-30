using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TciPM.Blazor.Shared.Models
{
    public class VisibleAuthUser
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

        public HashSet<Permission> Permissions { get; set; } = new HashSet<Permission>();
        public HashSet<ObjectId> Cities { get; set; } = new HashSet<ObjectId>();
        public string RestrictedIP { get; set; }
        public bool IsDailyCenterWorker { get; set; }
        public HashSet<ObjectId> AllowedDailyCenters { get; set; } = new HashSet<ObjectId>();
        public HashSet<EquipmentType> AllowedEquipmentTypes { get; set; } = new HashSet<EquipmentType>();
        public bool IsSuperAdmin { get; set; }

        public bool HasPermission(Permission perm)
        {
            return IsAdmin || Permissions.Contains(perm);
        }
    }
}
