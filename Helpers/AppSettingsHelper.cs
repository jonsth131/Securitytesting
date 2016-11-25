using System.Configuration;
using Securitytesting.Enums;

namespace Securitytesting.Helpers
{
    internal static class AppSettingsHelper
    {
        internal static string ReadString(AppSettings key)
        {
            var s = key.ToString();
            var appSettingsReader = new AppSettingsReader();
            return appSettingsReader.GetValue(s, typeof(string)) as string;
        }

        internal static int ReadInt(AppSettings key)
        {
            var s = key.ToString();
            var appSettingsReader = new AppSettingsReader();
            return (int) appSettingsReader.GetValue(s, typeof(int));
        }
    }
}
