using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxInvoiceBuildingView : DatatableAjaxModel
    {
        public string companyName { get; set; }
        public string invoiceTypeId { get; set; }
        public int invoiceStatusId { get; set; }
        public string invNo { get; set; }
        public string InvoiceCategory { get; set; }
    }
}