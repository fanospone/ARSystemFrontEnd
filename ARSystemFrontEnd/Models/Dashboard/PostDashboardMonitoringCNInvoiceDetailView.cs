using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardMonitoringCNInvoiceDetailView : PostDashboardView
    {
        public string InvNumber { get; set; }
        public string NoFaktur { get; set; }
        public string CompanyInvoice { get; set; }
        public string CustomerID { get; set; }
        public string Subject { get; set; }
        public string AmountInvoice { get; set; }
        public string CustomerName { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string OD_13 { get; set; }
        public string OD_46 { get; set; }
        public string OD_79 { get; set; }
        public string OD_9s { get; set; }
        public string vPKP { get; set; }
        public string Range { get; set; }
    }
}