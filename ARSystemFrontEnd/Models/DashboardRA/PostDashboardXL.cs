using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardXL : DatatableAjaxModel
    {
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public string StipCategory { get; set; }
        public string Year { get; set; }
        public string TenantType { get; set; }
        public string LeadTime { get; set; }
        public string ID { get; set; }
        public string StatusID { get; set; }
        public string ParentID { get; set; }
    }
}