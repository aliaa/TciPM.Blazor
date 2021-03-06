﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.Reflection;
using TciPM.Blazor.Shared.Utils;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    public abstract class EquipmentPM<Eq> where Eq : Equipment
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string SourceId { get; set; }

        public Eq Source { get; set; }

        public EquipmentPM() { }

        public EquipmentPM(Eq Source)
        {
            this.Source = Source;
            this.SourceId = Source.Id;
        }

        [DisplayName("نیاز به عملیات اصلاحی فوری دارد")]
        public bool NeedsEmergencyOperations { get; set; }

        [DisplayName("به سیستم آلارم ها متصل است")]
        public bool ConnectedToAlarmsSystem { get; set; }

        [DisplayName("نظافت")]
        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        public GoodBad Cleaning { get; set; }

        [DisplayName("روشنایی")]
        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        public GoodBad Lighting { get; set; }

        [DisplayName("تهویه")]
        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        public GoodBad Ventilation { get; set; }

        [DisplayName("فرم ظاهری کابلها")]
        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        public GoodBad CablesAppearance { get; set; }

        [DisplayName("کابل اضافه")]
        public bool ExtraCable { get; set; }

        [DisplayName("وسایل سایر واحدها")]
        public bool OtherUnitsEquipments { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("درصد سلامتی")]
        public virtual double HealthPercentage
        {
            get
            {
                int count = 0;
                double sum = 0;
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    var attr = prop.GetCustomAttribute<HealthParameterAttribute>();
                    if (attr != null)
                    {
                        sum += attr.GetHealth(prop, prop.GetValue(this)) * attr.Importance;
                        count += attr.Importance;
                    }
                }
                if (count == 0)
                    return 1;
                return sum / count;
            }
        }
    }
}