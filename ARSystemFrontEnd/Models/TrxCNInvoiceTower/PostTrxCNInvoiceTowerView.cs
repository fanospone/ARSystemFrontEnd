using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxCNInvoiceTowerView : DatatableAjaxModel
    {
        public string companyId { get; set; }
        public string invoiceTypeId { get; set; }
        public string operatorId { get; set; }
        public string invNo { get; set; }
    }
}