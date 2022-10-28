using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostProjectInformationView : DatatableAjaxModel
    {
        public string SoNumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string RegionalName { get; set; }

        public List<int> ListID { get; set; }
    }
}