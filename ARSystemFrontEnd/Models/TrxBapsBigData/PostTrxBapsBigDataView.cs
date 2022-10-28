using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBapsBigDataView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strStatusBAPS { get; set; }
        public string strPeriodInvoice { get; set; }
        public string strInvoiceType { get; set; }
        public string strCurrency { get; set; }
        public string strPONumber { get; set; }
        public string strBAPSNumber { get; set; }
        public string strSONumber { get; set; }
        public string strBapsType { get; set; }
        public string strSiteIdOld { get; set; }
        public int isReceive { get; set; }
        public List<int> excludedIDs { get; set; }
        public List<int> includedIDs { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
    }
}