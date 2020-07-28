using EasyMongoNet;
using MongoDB.Bson;

namespace TciPM.Blazor.Shared.Models
{
    [CollectionIndex(new string[] { nameof(User), nameof(Center) })]
    [CollectionSave(WriteLog = false)]
    public class TemporalPM : MongoEntity
    {
        public ObjectId User { get; set; }
        public ObjectId Center { get; set; }
        public EquipmentsPM PM { get; set; }
    }
}