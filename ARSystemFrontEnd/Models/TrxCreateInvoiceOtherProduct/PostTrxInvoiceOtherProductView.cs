using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxInvoiceOtherProductView : DatatableAjaxModel
    {
        public string soNumber { get; set; }
        public string invoiceNumber { get; set; }
    }
}