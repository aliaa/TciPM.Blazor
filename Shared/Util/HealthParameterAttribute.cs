using System;
using System.Linq;
using System.Reflection;

namespace TciPM.Blazor.Shared.Util
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class HealthParameterAttribute : Attribute
    {
        public int Importance { get; set; } = 1;
        public double MinOkRange { get; set; } = 0;
        public double MaxOkRange { get; set; } = 1;
        public double DateDaysOkRange { get; set; } = 30;
        public string[] EnumOkItems { get; set; } = new string[0];

        private static Type AttrType = typeof(HealthParameterAttribute);

        public float GetHealth(PropertyInfo prop, object value)
        {
            if (value is int)
            {
                double v = (int)value;
                if (v >= MinOkRange && v <= MaxOkRange)
                    return 1;
                return 0;
            }
            else if (value is float)
            {
                double v = (float)value;
                if (v >= MinOkRange && v <= MaxOkRange)
                    return 1;
                return 0;
            }
            else if(value is double)
            {
                double v = (double)value;
                if (v >= MinOkRange && v <= MaxOkRange)
                    return 1;
                return 0;
            }
            else if (value is DateTime)
            {
                if (DateDaysOkRange - (DateTime.Now - (DateTime)value).TotalDays > 0)
                    return 1;
                return 0;
            }
            else if(value is TimeSpan)
            {
                double totalSecods = ((TimeSpan)value).TotalSeconds;
                if (totalSecods >= MinOkRange && totalSecods <= MaxOkRange)
                    return 1;
                return 0;
            }
            else if(prop.PropertyType.IsEnum)
            {
                if (EnumOkItems.Contains(value.ToString()))
                    return 1;
                return 0;
            }
            throw new Exception("Property type is not valid for calculating health parameter!");
        }
    }
}