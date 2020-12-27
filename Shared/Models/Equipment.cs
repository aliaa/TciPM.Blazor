using EasyMongoNet;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{

    [CollectionIndex(new string[] { nameof(Name) })]
    [CollectionIndex(new string[] { nameof(Center) })]
    [CollectionSave(WriteLog = true)]
    [BsonKnownTypes(typeof(Diesel), typeof(RectifierAndBattery), typeof(Ups), typeof(AcSystem), typeof(AirConditioner))]
    public abstract class Equipment : MongoEntity
    {
        [Display(Name="نام")]
        public string Name { get; set; }

        [Display(Name="مرکز")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Center { get; set; }

        [DisplayName("تاریخ تولید")]
        public DateTime ProductionDate { get; set; }

        [DisplayName("تاریخ نصب")]
        public DateTime InstallationDate { get; set; }

        public bool Deleted { get; set; } = false;


        [DisplayName("برند")]
        public string Brand { get; set; }

        [DisplayName("مدل")]
        public string Model { get; set; }
    }
}