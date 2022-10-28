using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTaxInvoiceView : DatatableAjaxModel
    {
        public string invNo { get; set; }
        public string taxInvoiceNo { get; set; }
    }
}