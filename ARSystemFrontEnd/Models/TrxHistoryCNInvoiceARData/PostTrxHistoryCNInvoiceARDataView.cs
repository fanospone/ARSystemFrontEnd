using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxHistoryCNInvoiceARDataView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public string InvNo { get; set; }
        public string TaxNo { get; set; }
        public string CNStatus { get; set; }
        public string InvoiceTypeId { get; set; }
        public int ProccessType { get; set; }
        public string ReplacementStatus { get; set; }
        public DateTime? ReplaceDate { get; set; }
        public string ReplaceInvoice { get; set; }
    }
}