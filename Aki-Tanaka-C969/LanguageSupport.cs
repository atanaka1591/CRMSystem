using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki_Tanaka_C969
{
    public class LanguageSupport
    {
        //Returns regionInfo based on current region setting
        private static RegionInfo GetRegionInfo()
        {
            Microsoft.Win32.RegistryKey regKeyGeoId = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Control Panel\International\Geo");
            string geoID = (string)regKeyGeoId.GetValue("Nation");
            System.Collections.Generic.IEnumerable<System.Globalization.RegionInfo> allRegions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.ToString()));
            System.Globalization.RegionInfo regionInfo = allRegions.FirstOrDefault(r => r.GeoId == Int32.Parse(geoID));
            return regionInfo;
        }

        //Returns login page labels depending on regioninfo country - UNITED STATES and SPAIN region available
        public static List<String> GetLoginLabels()
        {
            var labels = new List<String>();
            if (GetRegionInfo().EnglishName == "United States")
            {
                labels.Add("Please log in");
                labels.Add("Username:");
                labels.Add("Password:");
                labels.Add("Log In");
            }
            else if (GetRegionInfo().EnglishName == "Spain")
            {
                labels.Add("Por favor Iniciar sesión");
                labels.Add("Nombre de usuario:");
                labels.Add("Contraseña:");
                labels.Add("Iniciar sesión");
            }
            else
            {
                labels.Add("Please log in");
                labels.Add("Username:");
                labels.Add("Password:");
                labels.Add("Log In");
            }
            return labels;
        }

        //Returns the login message labels that appears after clicking the login button
        public static List<String> GetLoginMessageLabels()
        {
            var labels = new List<String>();
            if (GetRegionInfo().EnglishName == "United States")
            {
                labels.Add("Invalid username or password.");
                labels.Add("Logging in...");
            }
            else if (GetRegionInfo().EnglishName == "Spain")
            {
                labels.Add("Usuario o contraseña invalido.");
                labels.Add("Iniciando sesión...");
            }
            else
            {
                labels.Add("Invalid username or password.");
                labels.Add("Logging in...");
            }
            return labels;
        }

    }
}
