using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TciPM.Blazor.Shared.Models
{
    public class CentersAggregatedData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("تعداد کل مراکز")]
        public int CentersCount { get; set; }

        [DisplayName("تعداد مراکز با اهمیت بالاتر از 5")]
        public int MoreThan5PriorityCentersCount { get; set; }

        [DisplayName("تعداد مراکز با اهمیت 5 و پایینتر")]
        public int LessThan5PriorityCentersCount { get; set; }

        [DisplayName("درصد PM به موقع کل مراکز")]
        public int? CentersOnTimePMPercent { get; set; }

        [DisplayName("درصد PM به موقع مراکز با اهمیت بالاتر از 5")]
        public int? MoreThan5PriorityCentersOnTimePM { get; set; }

        [DisplayName("درصد PM به موقع مراکز با اهمیت 5 و پایینتر")]
        public int? LessThan5PriorityCentersOnTimePM { get; set; }

        [DisplayName("تعداد PM تجهیزات انجام یافته")]
        public int EquipmentPMsCount { get; set; }

        [DisplayName("تعداد PM روزانه انجام یافته")]
        public int DailyPMsCount { get; set; }
    }
}