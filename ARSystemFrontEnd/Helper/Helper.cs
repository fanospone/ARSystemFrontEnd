using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;

using ARSystemFrontEnd.SecureAccessService;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Helper
{
    public static class Helper
    {
        public static string GetApplicationName()
        {
            return ConfigurationManager.AppSettings["ApplicationName"].ToString();
        }

        public static int GetApplicationID()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ApplicationID"]);
        }

        public static int GetIdleTime()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["IdleTime"]);
        }

        public static string GetIPAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        public static string GetDocPath()
        {
            return ConfigurationManager.AppSettings["DocPath"].ToString();
        }

	    public static string TbigSysDoc()
        {
            return ConfigurationManager.AppSettings["TbigSysDoc"].ToString();
        }

        public static string GetDocumentTeamMatePath()
        {
            return ConfigurationManager.AppSettings["DocTeamMatePath"].ToString();
        }

        public static string GetFileTimeStamp(string fileName)
        {
            string fileExtension = fileName.Substring(fileName.LastIndexOf("."));
            return DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;
        }

        public static string ConvertDateTimeToIndDate(DateTime Date)
        {
            if (Date == null) return "";
            var month = "";
            switch (Date.Month)
            {
                case 1:
                    month = "Januari";
                    break;
                case 2:
                    month = "Februari";
                    break;
                case 3:
                    month = "Maret";
                    break;
                case 4:
                    month = "April";
                    break;
                case 5:
                    month = "Mei";
                    break;
                case 6:
                    month = "Juni";
                    break;
                case 7:
                    month = "Juli";
                    break;
                case 8:
                    month = "Agustus";
                    break;
                case 9:
                    month = "September";
                    break;
                case 10:
                    month = "Oktober";
                    break;
                case 11:
                    month = "November";
                    break;
                case 12:
                    month = "Desember";
                    break;
            }
            return ((Date.Day < 10) ? "0" + Date.Day : Date.Day.ToString()) + "-" + month + "-" + Date.Year;
        }

        public static string ConvertDateTimeToSQLFormat(string Date)
        {
            string[] Datetemp;
            Datetemp = Date.Split('/');
            string Month = Datetemp[1][0].ToString() == "0" ? Datetemp[1][1].ToString() : Datetemp[1];
            return Month + "/" + Datetemp[0] + "/" + Datetemp[2];
        }

        public static bool HitLog(RouteData routeData)
        {
            var applicationID = GetApplicationID();
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var url = HttpContext.Current.Request.Url.AbsolutePath;
            var message = String.Format("{0};{1};{2}", controllerName, actionName, url);

            if (url == "/Error/NotFound")
                return false;

            using (var client = new AccessServiceClient())
            {
                if (UserManager.User == null)
                    client.CreateHitLog(string.Empty, applicationID, message);
                else
                    client.CreateHitLog(UserManager.User.UserToken, applicationID, message);
            }
            return true;
        }
    }

    public static class CacheExtensions
    {
        public static T GetOrStore<T>(this Cache cache, string key, Func<T> generator)
        {
            var result = cache[key];
            if (result == null)
            {
                result = generator();
                cache[key] = result;
            }
            return (T)result;
        }
    }
}