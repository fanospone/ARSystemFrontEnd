using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxInvoiceBuildingPosting : DatatableAjaxModel
    {
        public int trxInvoiceHeaderID { get; set; }
        public string invNo { get; set; }
        public string invTemp { get; set; }
        public string subject { get; set; }
        public string invoiceDate { get; set; }
        public string signature { get; set; }
        public string remark { get; set; }
    }
}