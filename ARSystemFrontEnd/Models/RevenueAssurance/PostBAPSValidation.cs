using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAPSValidation : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strCustomerId { get; set; }
        public string strGroupBy { get; set; }
        public string strProductId { get; set; }
        public string strBapsTypeId { get; set; }
        public string strBulkNumber { get; set; }
        public string strRegionId { get; set; }
        public string strSoNumber { get; set; }
        public string strStipCategory { get; set; }
        public int strBulkID { get; set; }
        public string strSiteID { get; set; }
        public string strSiteName { get; set; }
        public string strDataType { get; set; }
        public int strIDTrx { get; set; }
        public string strSiteIDCustomer { get; set; }
        public string strAction { get; set; }
        public string strTenantTypeID { get; set; }
        public string strBapsType { get; set; }
        public string strCategory { get; set; }
        public string strStipID { get; set; }
        public string strSiroID { get; set; }
        public DateTime? strStartBaukDoneDate { get; set; }
        public DateTime? strEndBaukDoneDate { get; set; }
        // =================== validation =============================//
        public int strStipSiro { get; set; }
        public DateTime? strIssuingDate { get; set; }
        public string strPoNumber { get; set; }
        public DateTime? strPoDate { get; set; }
        public string strMLAnumber { get; set; }
        public DateTime? strMLAdate { get; set; }
        public string strBaukNumber { get; set; }
        public DateTime? strBaukdate { get; set; }
        public DateTime? strStartLeasePriod { get; set; }
        public DateTime? strEndLeasePriod { get; set; }
        public string strInvoiceTypeID { get; set; }
        public DateTime? strStartEffectivePeriod { get; set; }
        public DateTime? strEndEffectivePeriod { get; set; }
        public decimal strBaseLeasePrice { get; set; }
        public decimal strServicePrice { get; set; }
        public decimal strTotalLeasePrice { get; set; }
        public decimal strInitialPoAmount { get; set; }
        public decimal strPoAmount { get; set; }
        public string strCorresRegionOpr { get; set; }
        public string strApprOprID { get; set; }

        public int strBaukDoneYear { get; set; }
        public List<string> strSONumberMultiple { get; set; }

        public ARSystemService.mstBaps bapsValidation = new ARSystemService.mstBaps();
        public ARSystemService.trxFreeRentBaps FreeRent = new ARSystemService.trxFreeRentBaps();
    }


}