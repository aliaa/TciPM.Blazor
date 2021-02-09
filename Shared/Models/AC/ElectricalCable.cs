using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models.AC
{
    public class ElectricalCable : MongoEntity
    {
        public enum CableType
        {
            [Display(Name="سه فاز همسان")]
            Sync3Phase,
            [Display(Name="سه فاز غیرهمسان")]
            Async3Phase,
            [Display(Name="تک رشته")]
            MonoString,
        }

        [DisplayName("نوع")]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public CableType Type { get; set; }

        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("سطح مقطع (mm2)")]
        public float CrossSection { get; set; }

    }
}