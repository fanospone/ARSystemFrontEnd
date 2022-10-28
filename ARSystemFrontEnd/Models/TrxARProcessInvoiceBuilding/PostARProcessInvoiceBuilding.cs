using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostARProcessInvoiceBuilding : DatatableAjaxModel
    {
        public string invoiceTypeId { get; set; }
        public string customerName { get; set; }
        public string invCompanyId { get; set; }
        public string invNo { get; set; }
        public DateTime? receiptDateFrom { get; set; }
        public DateTime? receiptDateTo { get; set; }
        public string StatusReceipt { get; set; }
    }
}