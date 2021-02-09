using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Models.Equipments;

namespace TciPM.Blazor.Shared.Models
{
    public abstract class BaseAuthUser
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

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

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Cities { get; set; } = new List<string>();
        
        [Display(Name = "IP محدود شده")]
        public string RestrictedIP { get; set; }

        [Display(Name = "کارگزار مراکز کم ظرفیت است؟")]
        public bool IsDailyCenterWorker { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> AllowedDailyCenters { get; set; } = new List<string>();
        public List<EquipmentType> AllowedEquipmentTypes { get; set; } = new List<EquipmentType>();
        public bool IsSuperAdmin { get; set; }

        public bool HasPermission(Permission perm)
        {
            return IsAdmin || Permissions.Contains(perm);
        }
    }
}
