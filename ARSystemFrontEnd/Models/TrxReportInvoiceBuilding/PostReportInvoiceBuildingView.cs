using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReportInvoiceBuildingView : DatatableAjaxModel
    {
        public DateTime invPrintDateFrom { get; set; }
        public DateTime invPrintDateTo { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int week { get; set; }
        public string InvNo { get; set; }
    }
}