using System.ComponentModel;

namespace TciPM.Blazor.Shared.Models
{
    public class Ups : Equipment
    {
        [DisplayName("توان (KVA)")]
        public int Power { get; set; }

        [DisplayName("تعداد باتری ها در هر سری")]
        public int CellsCount { get; set; }

        [DisplayName("تعداد سری باتری ها")]
        public int SeriesCount { get; set; }

        [DisplayName("ولتاژ هر سلول باتری")]
        public float CellsNormalVoltage { get; set; }

        [DisplayName("ظرفیت باتری ها")]
        public float BatteriesCapacity { get; set; }

        [DisplayName("نوع برق ورودی")]
        public ElectricityType InputType { get; set; }

        [DisplayName("نوع برق خروجی")]
        public ElectricityType OutputType { get; set; }
    }
}