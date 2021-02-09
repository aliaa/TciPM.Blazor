using System.ComponentModel;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    public class UpsPM : EquipmentPM<Ups>
    {
        public UpsPM() { }
        public UpsPM(Ups Source) : base(Source) { }

        [DisplayName("ولتاژ AC ورودی")]
        public float EntryACVoltage { get; set; }

        [DisplayName("ولتاژ AC ورودی (R)")]
        public float EntryACVoltageR { get; set; }

        [DisplayName("ولتاژ AC ورودی (S)")]
        public float EntryACVoltageS { get; set; }

        [DisplayName("ولتاژ AC ورودی (T)")]
        public float EntryACVoltageT { get; set; }

        [DisplayName("ولتاژ AC خروجی")]
        public float OutACVoltage { get; set; }

        [DisplayName("ولتاژ AC خروجی (R)")]
        public float OutACVoltageR { get; set; }

        [DisplayName("ولتاژ AC خروجی (S)")]
        public float OutACVoltageS { get; set; }

        [DisplayName("ولتاژ AC خروجی (T)")]
        public float OutACVoltageT { get; set; }

        [DisplayName("ولتاژ DC خروجی")]
        public float OutDCVoltage { get; set; }

        [DisplayName("جریان ورودی")]
        public float EntryCurrent { get; set; }

        [DisplayName("جریان ورودی (R)")]
        public float EntryCurrentR { get; set; }

        [DisplayName("جریان ورودی (S)")]
        public float EntryCurrentS { get; set; }

        [DisplayName("جریان ورودی (T)")]
        public float EntryCurrentT { get; set; }

        [DisplayName("جریان مصرفی")]
        public float OutCurrent { get; set; }

        [DisplayName("جریان مصرفی (R)")]
        public float OutCurrentR { get; set; }

        [DisplayName("جریان مصرفی (S)")]
        public float OutCurrentS { get; set; }

        [DisplayName("جریان مصرفی (T)")]
        public float OutCurrentT { get; set; }


        [DisplayName("کارکرد نرمال بعد از قطع ورودی AC")]
        public bool NormalUsageAfterAcCut { get; set; }

        public float[] SeriesCurrent { get; set; }

        public float[][] CellsVoltages { get; set; }

        [DisplayName("ولتاژ کل باتری ها")]
        public float TotalCellsVoltage { get; set; }

        [DisplayName("فرکانس")]
        public float Frequency { get; set; }

        [DisplayName("عملکرد سیستم آلارم")]
        public GoodBad AlarmSystemStatus { get; set; }
    }
}