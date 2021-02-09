using EasyMongoNet;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Models.Equipments.PM;

namespace TciPM.Blazor.Shared.Models
{
    [CollectionIndex(new string[] { nameof(CenterId) })]
    [CollectionIndex(new string[] { nameof(ReportingUser) })]
    [CollectionIndex(new string[] { nameof(SubmitDate) })]
    [CollectionOptions(Name = "CenterPM")]
    public class EquipmentsPM : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [Display(Name = "مرکز")]
        public string CenterId { get; set; }

        [Display(Name = "تاریخ ثبت الکترونیکی")]
        public DateTime SubmitDate { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ انجام PM")]
        public DateTime PmDate { get; set; }

        [Display(Name = "تاریخ تغییر")]
        public DateTime EditDate { get; set; } = DateTime.Now;

        [BsonRepresentation(BsonType.ObjectId)]
        [Display(Name = "کاربر ثبت کننده")]
        public string ReportingUser { get; set; }

        public List<DieselPM> DieselsPM { get; set; } = new List<DieselPM>();

        public List<RectifierPM> RectifiersPM { get; set; } = new List<RectifierPM>();

        public List<BatteryPM> BatteriesPM { get; set; } = new List<BatteryPM>();

        public List<UpsPM> UpsPM { get; set; } = new List<UpsPM>();

        [Display(Name = "امتیاز کلی مرکز")]
        public int TotalRate { get; set; }

        public string IP { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        [Display(Name = "درصد سلامت مرکز")]
        public double HealthPercentage
        {
            get
            {
                double sum = 0;
                int count = 0;
                foreach (DieselPM dpm in DieselsPM)
                {
                    sum += dpm.HealthPercentage;
                    count++;
                }
                foreach (RectifierPM rpm in RectifiersPM)
                {
                    sum += rpm.HealthPercentage;
                    count++;
                }
                foreach (BatteryPM bpm in BatteriesPM)
                {
                    sum += bpm.HealthPercentage;
                    count++;
                }
                if (count == 0)
                    return 1;
                return sum / count;
            }
        }

        //public static bool PmExistsForCenterInLastHours(IReadOnlyDbContext db, ObjectId centerId, int hours = 24)
        //{
        //    DateTime lastHours = DateTime.Now.AddHours(-Math.Abs(hours));
        //    return db.Any<EquipmentsPM>(pm => pm.CenterId == centerId && pm.SubmitDate >= lastHours);
        //}
    }
}