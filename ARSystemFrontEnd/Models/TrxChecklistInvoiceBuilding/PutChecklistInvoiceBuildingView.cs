using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PutChecklistInvoiceBuildingView
    {
        public int trxInvoiceHeaderId { get; set; }
        public string taxInvoiceNo { get; set; }
        public int mstInvoiceCategoryId { get; set; }
    }
}