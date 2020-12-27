using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TciPM.Blazor.Shared.Models
{
    public class Comment
    {
        public DateTime Date { get; set; } = DateTime.Now;

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool PmWriterSeen { get; set; } = false;
    }
}