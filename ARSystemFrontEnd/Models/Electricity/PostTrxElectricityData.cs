using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxElectricityData : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strStatus { get; set; }
        public string strPeriodInvoice { get; set; }
        public string strInvoiceType { get; set; }
        public string strCurrency { get; set; }
        public string strPONumber { get; set; }
        public string strPICA { get; set; }
        public string strSONumber { get; set; }
        public string strRejectRemarks { get; set; }
        public string strSiteIdOld { get; set; }
        public int isReceive { get; set; }
        public int isReject { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public string strCreatedBy { get; set; }
        public string strSiteName { get; set; }
        public string strAccountNumber { get; set; }
        public string strAccountName { get; set; }
        public string strBankName { get; set; }
        public string strSiteIDOpr { get; set; }
        public string strVoucherNumber { get; set; }
        public string strDescription { get; set; }
        public string strYearPeriod { get; set; }
        public string strRegion { get; set; }
        public List<int> ListId { get; set; }
    }
}