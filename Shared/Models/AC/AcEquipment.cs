using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace TciPM.Blazor.Shared.Models.AC
{
    [BsonKnownTypes(typeof(ElectricalEnclosure), typeof(InnerElectricalEnclosure), typeof(CapacitorBank), typeof(DieselAcRef))]
    public abstract class AcEquipment
    {
        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("محل نصب")]
        public string Place { get; set; }
        
        [DisplayName("نوع کابل ورودی")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EntryCable { get; set; }

        [DisplayName("تعداد کابل")]
        public int EntryCableCount { get; set; }

        [DisplayName("طول کابل ورودی")]
        public float EntryCableLength { get; set; }

        [DisplayName("کلید ورودی")]
        public ElectricalSwitch EntrySwitch { get; set; }
    }
}