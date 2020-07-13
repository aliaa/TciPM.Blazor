using MongoDB.Bson;

namespace TciPM.Blazor.Shared.Models
{
    public class DieselAcRef : AcEquipment
    {
        public ObjectId DieselId { get; set; }
    }
}