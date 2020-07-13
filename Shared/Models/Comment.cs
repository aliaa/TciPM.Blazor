using MongoDB.Bson;
using System;

namespace TciPM.Blazor.Shared.Models
{
    public class Comment
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public ObjectId UserId { get; set; }
        public string Message { get; set; }
        public bool PmWriterSeen { get; set; } = false;
    }
}