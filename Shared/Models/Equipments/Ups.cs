using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Models.Equipments
{
    public class Ups : Equipment
    {
        [DisplayName("توان (KVA)")]
        [Required]
        public int Power { get; set; }

        [DisplayName("تعداد باتری ها در هر سری")]
        [Range(minimum:1, maximum:100)]
        [Required]
        public int CellsCount { get; set; }

        [DisplayName("تعداد سری باتری ها")]
        [Range(minimum:1, maximum:10)]
        [Required]
        public int SeriesCount { get; set; }

        [DisplayName("ولتاژ هر سلول باتری")]
        [Required]
        public float CellsNormalVoltage { get; set; }

        [DisplayName("ظرفیت باتری ها")]
        [Required]
        public float BatteriesCapacity { get; set; }

        [DisplayName("نوع برق ورودی")]
        public ElectricityType InputType { get; set; }

        [DisplayName("نوع برق خروجی")]
        public ElectricityType OutputType { get; set; }
    }
}