﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TciPM.Blazor.Shared.Models
{
    public class DieselAcRef : AcEquipment
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string DieselId { get; set; }
    }
}