using EasyMongoNet;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using TciPM.Blazor.Shared.Utils;

namespace TciPM.Blazor.Shared.Models.Equipments.PM
{
    [CollectionIndex(new string[] { nameof(SourceId) })]
    [BsonIgnoreExtraElements]
    public class BatteryPM : EquipmentPM<RectifierAndBattery>
    {
        public const float MIN_VOLTAGE_2V = 1.9f;
        public const float NORMAL_VOLTAGE_2V = 1.95f;
        public const float MAX_VOLTAGE_2V = 2.25f;
        public const float MIN_VOLTAGE_12V = 11f;
        public const float NORMAL_VOLTAGE_12V = 12f;
        public const float MAX_VOLTAGE_12V = 13f;
        public const float MIN_DENSITY = 1.21f;
        public const float NORMAL_DENSITY = 1.23f;
        public const float MAX_DENSITY = 1.26f;

        public static float GetBatteryMinVoltage(RectifierAndBattery.CellCountEnum cellCount)
        {
            return cellCount switch
            {
                RectifierAndBattery.CellCountEnum._2 or RectifierAndBattery.CellCountEnum._4 => MIN_VOLTAGE_12V,
                RectifierAndBattery.CellCountEnum._8 => MIN_VOLTAGE_12V / 2,
                RectifierAndBattery.CellCountEnum._12 or RectifierAndBattery.CellCountEnum._24 or RectifierAndBattery.CellCountEnum._25 => MIN_VOLTAGE_2V,
                _ => throw new NotImplementedException(),
            };
        }

        public static float GetBatteryMaxVoltage(RectifierAndBattery.CellCountEnum cellCount)
        {
            return cellCount switch
            {
                RectifierAndBattery.CellCountEnum._2 or RectifierAndBattery.CellCountEnum._4 => MAX_VOLTAGE_12V,
                RectifierAndBattery.CellCountEnum._8 => MAX_VOLTAGE_12V / 2,
                RectifierAndBattery.CellCountEnum._12 or RectifierAndBattery.CellCountEnum._24 or RectifierAndBattery.CellCountEnum._25 => MAX_VOLTAGE_2V,
                _ => throw new NotImplementedException(),
            };
        }

        public static float GetBatteryNormalVoltage(RectifierAndBattery.CellCountEnum cellCount)
        {
            return cellCount switch
            {
                RectifierAndBattery.CellCountEnum._2 or RectifierAndBattery.CellCountEnum._4 => NORMAL_VOLTAGE_12V,
                RectifierAndBattery.CellCountEnum._8 => NORMAL_VOLTAGE_12V / 2,
                RectifierAndBattery.CellCountEnum._12 or RectifierAndBattery.CellCountEnum._24 or RectifierAndBattery.CellCountEnum._25 => NORMAL_VOLTAGE_2V,
                _ => throw new NotImplementedException(),
            };
        }

        [BsonIgnoreExtraElements]
        public class BatterySeriesPM
        {
            public BatterySeriesPM() { }
            public BatterySeriesPM(int cellCount)
            {
                Voltages = new float[cellCount];
                Densities = new float[cellCount];
            }

            public RectifierAndBattery.CellCountEnum CellCount =>
                (RectifierAndBattery.CellCountEnum)Enum.Parse(typeof(RectifierAndBattery.CellCountEnum), "_" + Voltages.Length);

            public float[] Voltages { get; set; }

            public float[] Densities { get; set; }

            [Display(Name = "آب مقطر اضافه شد")]
            public bool DistilledWaterAdded { get; set; }

            [Display(Name = "درجه دما")]
            [HealthParameter(MinOkRange = 0, MaxOkRange = 27)]
            public float Temperature { get; set; }

            [Display(Name = "جریان خروجی سری")]
            public float OutputCurrent { get; set; }

            [Display(Name = "تعداد باتری مستعمل")]
            [HealthParameter(MaxOkRange = 0)]
            public int OldBatteryCount { get; set; }

            public string Description { get; set; }

            public float MinVoltage => GetBatteryMinVoltage(CellCount);

            public float MaxVoltage => GetBatteryMaxVoltage(CellCount);

            public float NormalVoltage => GetBatteryNormalVoltage(CellCount);

            public double HealthPercentage
            {
                get
                {
                    int count = 2;
                    double sum = 0;
                    if (Temperature > 0 && Temperature <= 27)
                        sum++;
                    //TODO: Add also OutputCurrent parameter
                    if (OldBatteryCount == 0)
                        sum++;

                    foreach (float v in Voltages)
                    {
                        if (v >= MinVoltage && v < NormalVoltage)
                            sum += 0.2;
                        else if (v >= NormalVoltage && v <= MaxVoltage)
                            sum += 1;
                        count++;
                    }
                    foreach (float d in Densities)
                    {
                        if (d >= MIN_DENSITY && d < NORMAL_DENSITY)
                            sum += 0.2;
                        else if (d >= NORMAL_DENSITY && d <= MAX_DENSITY)
                            sum += 1;
                        count++;
                    }
                    return sum / count;
                }
            }
        }

        public List<BatterySeriesPM> Series { get; set; } = new List<BatterySeriesPM>();

        public BatteryPM() { }

        public BatteryPM(RectifierAndBattery Source) : base(Source) { }

        [Display(Name = "درصد سلامتی")]
        [JsonIgnore]
        public override double HealthPercentage => Series.Count > 0 ? Series.Sum(bs => bs.HealthPercentage) / Series.Count : 0;

        [Display(Name = "تعداد سلولهای دارای ولتاژ مشکل دار")]
        [JsonIgnore]
        public int HavingVoltageProblemCellsCount
        {
            get
            {
                int sum = 0;
                var source = (RectifierAndBattery)Source;
                foreach (var serie in Series)
                    sum += serie.Voltages.Count(v => v > 0 && (v > serie.MaxVoltage || v < serie.MinVoltage));
                return sum;
            }
        }

        [Display(Name = "تعداد سلولهای دارای غلظت مشکل دار")]
        [JsonIgnore]
        public int HavingDensityProblemCellsCount => Series.Sum(bs => bs.Densities.Count(d => d > 0 && (d > MAX_DENSITY || d < MIN_DENSITY)));

        [Display(Name = "میانگین دما")]
        [JsonIgnore]
        public float TemperatureAverage => Series.Average(bs => bs.Temperature);

        [Display(Name = "تعداد باتری مستعمل")]
        [JsonIgnore]
        public int OldBatteriesCount => Series.Sum(s => s.OldBatteryCount);

        [Display(Name = "انحراف معیار جریان خروجی سری ها")]
        [JsonIgnore]
        public float SeriesOutputCurrentDeviation => UtilsX.CalculateStandardDeviation(Series.Select(s => s.OutputCurrent).ToArray());

        //[Display(Name="انحراف معیار جمع ولتاژ سری ها")]
        //public float SeriesVoltageSumDeviation => UtilsX.CalculateStandardDeviation(Series.Select(s => s.Voltages))
    }

}