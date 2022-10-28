using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardARMonitoringAgingView : PostDashboardView
    {
        public string CompanyId { get; set; }
        public string EndDate { get; set; }
        public string OperatorId { get; set; }
        public string InvoiceType { get; set; }
        public string AmountType { get; set; }
        public string Status { get; set; }
        public string vPKP { get; set; }
    }
}