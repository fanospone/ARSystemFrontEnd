using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Helper
{
    public static class UrlHelperExtensions
    {
        /* ## Cache Busting ##
         * Usage        : <script src="@Url.ContentVersion("~/scripts/RevenueSystem/ReportSummary.js")" type="text/javascript"></script>
         * Result       : .../scripts/RevenueSystem/ReportSummary.js?v=637251289453824005
         * Descriptionn : append version static file for consistent update delivery.
         */
        public static string ContentVersion(this UrlHelper helper,string rootRelativePath)
        {
            rootRelativePath = rootRelativePath.Replace("~","");
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                var absolute = HostingEnvironment.MapPath("~" + rootRelativePath);
                var date = File.GetLastWriteTime(absolute);

                var path = rootRelativePath;
                if (HostingEnvironment.ApplicationVirtualPath == "/" && path.Length > 0 && path[0] == '/')
                {
                    path = path.Substring(1, path.Length - 1);
                }

                var result = HostingEnvironment.ApplicationVirtualPath + path + "?v=" + date.Ticks;
                HttpRuntime.Cache.Insert(rootRelativePath, result, new CacheDependency(absolute));
            }

            return HttpRuntime.Cache[rootRelativePath] as string;
        }
    }
}