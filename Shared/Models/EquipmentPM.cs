using EasyMongoNet;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Reflection;
using TciPM.Blazor.Shared.Util;

namespace TciPM.Blazor.Shared.Models
{
    public abstract class EquipmentPM
    {
        [JsonConverter(typeof(ObjectIdJsonConverter))]
        public ObjectId SourceId { get; set; }

        public Equipment Source { get; set; }

        public EquipmentPM(Equipment Source)
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

        [DisplayName("کابل اضافه")]
        public bool ExtraCable { get; set; }

        [DisplayName("وسایل سایر واحدها")]
        public bool OtherUnitsEquipments { get; set; }

        [DisplayName("فرم ظاهری کابلها")]
        [HealthParameter(EnumOkItems = new string[] { nameof(GoodBad.Good) })]
        public GoodBad CablesAppearance { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("درصد سلامتی")]
        public virtual double HealthPercentage
        {
            get
            {
                int count = 0;
                double sum = 0;
                Type healthParamType = typeof(HealthParameterAttribute);
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    HealthParameterAttribute attr = (HealthParameterAttribute)prop.GetCustomAttribute(healthParamType);
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