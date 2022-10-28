using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostNewPICA
    {
        public string Process { get; set; }
        public string TypePICA { get; set; }
        public string CategoryPICA { get; set; }
        public string Description { get; set; }
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string SIRO { get; set; }
    }
}