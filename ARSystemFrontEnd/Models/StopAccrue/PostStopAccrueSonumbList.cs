using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostStopAccrueSonumbList : DatatableAjaxModel
    {
        public string SONumber { get; set; }
        public string SiteName { get; set; }
        public string CompanyID { get; set; }
        public string SiteID { get; set; }
        public string CustomerID { get; set; }
        public DateTime? RFIDone { get; set; }
        public int RegionID { get; set; }
        public int? ProductID { get; set; }
    }
}