using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models
{
    public class GasCable : Equipment
    {
        public class GasCableDestination
        {
            [Display(Name = "آدرس")]
            public string Address { get; set; }
            [Display(Name = "طول کابل (متر)")]
            public int Length { get; set; }
        }

        [Display(Name = "متصل به کمپرسور")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [Required(ErrorMessage = "لطفا کمپرسور متصل را انتخاب کنید و یا ابتدا کمپرسور را تعریف کنید.")]
        public string ConnectedCompressor { get; set; }

        [Display(Name = "ردیف")]
        public int Index { get; set; }

        [Display(Name = "تعداد زوج سیم")]
        public int WirePairsCount { get; set; }

        [Display(Name = "قطر سیم")]
        [Range(minimum: 4, maximum: 6, ErrorMessage = "قطر سیم بایستی بین 4 و 6 باشد.")]
        public int WiresDiameter { get; set; } = 4;

        public override string Name
        {
            get => Index + "." + WirePairsCount + "." + WiresDiameter.ToString("D2");
            set { }
        }

        [Display(Name = "انشعاب")]
        public GasCableDestination Branch { get; set; }

        public List<GasCableDestination> Destinations { get; set; } = new List<GasCableDestination> { new GasCableDestination() };
    }
}
