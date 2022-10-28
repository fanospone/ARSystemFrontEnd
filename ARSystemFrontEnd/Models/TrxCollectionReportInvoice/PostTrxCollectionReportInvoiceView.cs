using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxCollectionReportInvoiceView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strPaidStatus { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public string strStartPaidDate { get; set; }
        public string strEndPaidDate { get; set; }
        public int intInvoiceCategory { get; set; }
        public int intCustomerId { get; set; }
        public string InvNo { get; set; }

    }
}