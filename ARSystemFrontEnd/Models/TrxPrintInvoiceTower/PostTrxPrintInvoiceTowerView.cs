using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxPrintInvoiceTowerView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strInvoiceType { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public List<int> HeaderId { get; set; }
        public List<int> CategoryId { get; set; }
        public List<int> isCNInvoice { get; set; }
        public int PICAReprintID { get; set; }
        public string ReprintRemarks { get; set; }
        public string RemarksPrint { get; set; }
        public string InvNo { get; set; }
        public int intmstInvoiceStatusId { get; set; }
        public int mstPICATypeID { get; set; }
        public int mstPICADetailID { get; set; }
        public string ApprovalStatus { get; set; }

        public int invoiceManual { get; set; }

        public bool IsCover { get; set; } /* Edd By MTR */

    }
}