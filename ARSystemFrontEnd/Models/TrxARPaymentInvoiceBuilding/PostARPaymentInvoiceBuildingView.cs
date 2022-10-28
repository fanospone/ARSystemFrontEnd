using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostARPaymentInvoiceBuildingView : DatatableAjaxModel
    {
        public string invoiceTypeId { get; set; }
        public string customerName { get; set; }
        public string invCompanyId { get; set; }
        public string invNo { get; set; }
    }
}