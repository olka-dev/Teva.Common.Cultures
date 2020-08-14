using System.Globalization;
using System.Text.Json;

namespace Teva.Common.Cultures
{
    public static class CultureHelper
    {
        public static CultureInfo GetCulture(string CultureName)
        {
            return CachedCultures.GetOrAdd(CultureName, GetCultureUncached);
        }

        public static CultureInfo GetCulture(int lcid)
        {
            return GetCulture(new CultureInfo(lcid, false).Name);
        }

        private static CultureInfo GetCultureUncached(string CultureName)
        {
            var SerializedCulture = GetSerializedCulture(CultureName);
            var Culture = (CultureInfo)new CultureInfo(CultureName).Clone();
            if (SerializedCulture != null)
            {
                SerializedCulture.NumberFormatInfo.PopulateIntoCultureInfo(Culture);
                SerializedCulture.DateTimeFormatInfo.PopulateIntoCultureInfo(Culture);
            }
            return Culture;
        }
        private static SerializedCulture GetSerializedCulture(string CultureName)
        {
            using (var Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Teva.Common.Cultures.cultures." + CultureName.ToLower() + ".json"))
            {
                if (Stream == null)
                    return null;
                using (var streamReader = new System.IO.StreamReader(Stream, System.Text.Encoding.UTF32))
                    return JsonSerializer.Deserialize<SerializedCulture>(streamReader.ReadToEnd());
            }
        }
        public static void ClearCache()
        {
            CachedCultures.Clear();
        }
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, CultureInfo> CachedCultures = new System.Collections.Concurrent.ConcurrentDictionary<string, CultureInfo>();
    }
}
