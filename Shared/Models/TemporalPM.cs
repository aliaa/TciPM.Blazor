using EasyMongoNet;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TciPM.Blazor.Shared.Models
{
    [CollectionIndex(new string[] { nameof(User), nameof(Center) })]
    [CollectionSave(WriteLog = false)]
    public class TemporalPM : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string User { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Center { get; set; }
        public EquipmentsPM PM { get; set; }
    }
}