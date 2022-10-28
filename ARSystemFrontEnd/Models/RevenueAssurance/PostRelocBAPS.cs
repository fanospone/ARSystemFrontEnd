using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostRelocBAPS : DatatableAjaxModel
    {
        public string Customer { get; set; }
        public string Company { get; set; }
        public string Stip { get; set; }
        public string Tenant { get; set; }
        public string SONumber { get; set; }
    }

    public class PostTrackRecurring : DatatableAjaxModel
    {
        public string CustomerID { get; set; }
        public string CompanyInvoice { get; set; }
        public string ActivityID { get; set; }
        public string Term { get; set; }
        public string StipSiro { get; set; }
        public string SONumber { get; set; }
        public string mstBapsID { get; set; }
        public string TransactionID { get; set; }
    }
}