using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardMonitoringCNInvoiceView : PostDashboardView
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? OD_13 { get; set; }
        public int? OD_46 { get; set; }
        public int? OD_79 { get; set; }
        public int? OD_9s { get; set; }
        public int? GrandTotal { get; set; }
        public string cnIDCollection_13 { get; set; }
        public string cnIDCollection_46 { get; set; }
        public string cnIDCollection_79 { get; set; }
        public string cnIDCollection_9s { get; set; }
        public string Status { get; set; }
        public string Periode { get; set; }
        public string vPKP { get; set; }
    }
}