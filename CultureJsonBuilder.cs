﻿using System.Text.Json;

namespace Teva.Common.Cultures
{
    public static class CultureJsonBuilder
    {
        public static void ExportCultures()
        {
            foreach (var Culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures))
            {
                if (!string.IsNullOrEmpty(Culture.Name))
                    ExportCulture(Culture);
            }
        }
        static void ExportCulture(System.Globalization.CultureInfo Culture)
        {
            var Details = new SerializedCulture
            {
                NumberFormatInfo = new SerializedNumberFormatInfo(),
                DateTimeFormatInfo = new SerializedDateTimeFormatInfo()
            };
            Details.NumberFormatInfo.PopulateFromCultureInfo(Culture);
            Details.DateTimeFormatInfo.PopulateFromCultureInfo(Culture);
            var JSON = JsonSerializer.Serialize(Details);
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(System.AppContext.BaseDirectory, "cultures"));
            System.IO.File.WriteAllText(System.IO.Path.Combine(System.AppContext.BaseDirectory, "cultures", Culture.Name.ToLower() + ".json"), JSON, System.Text.Encoding.UTF32);
        }
    }
}
