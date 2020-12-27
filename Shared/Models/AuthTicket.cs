using EasyMongoNet;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TciPM.Blazor.Shared.Models
{
    //[Serializable]
    [CollectionIndex(new string[] { nameof(Ticket) })]
    [CollectionIndex(new string[] { nameof(Time)}, ExpireAfterSeconds = 7200)]
    public class AuthTicket : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string User { get; set; }
        public string Ticket { get; set; } = Guid.NewGuid().ToString();
        public DateTime Time { get; set; } = DateTime.Now;
    }
}