using System;

namespace TciPM.Blazor.Client.Utils
{
    public class ColorUtils
    {
        public static string GetColorOfPercent(int? percent, int saturation = 74, int luminance = 85)
        {
            if (percent == null)
                return "initial";
            if (percent > 100)
                percent = 100;
            if (percent < 0)
                percent = 0;
            int hue = (int)Math.Round(percent.Value * 1.2);
            return GetHslColor(hue, saturation, luminance);
        }

        public static string GetHslColor(int hue, int saturation, int luminance) => $"hsl({hue}, {saturation}%, {luminance}%)";
    }
}
