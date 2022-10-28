using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxPostingInvoiceTowerView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strInvoiceType { get; set; }
        public int intmstInvoiceStatusId { get; set; }
        public string invNo { get; set; }
        public int invoiceManual { get; set; }

    }
}