using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostVwChangePPHFinal : DatatableAjaxModel
    {
        public string strInvoiceNumber { get; set; }
        public string strSONumber { get; set; }
        public string strBAPSNumber { get; set; }
        public string strBAPSType { get; set; }
        public string strBAPSPeriod { get; set; }
        public string strPONumber { get; set; }
        public string strSiteID { get; set; }
        public string strSiteName { get; set; }
        public string strSiteIDOpr { get; set; }
        public string strSiteNameOpr { get; set; }
        public string strType { get; set; }
        public string strOperatorID { get; set; }
        public string strSTIPSiroID { get; set; }
        public string strCompanyID { get; set; }
        public string strCompany { get; set; }
        public string strStartDateInvoice { get; set; }
        public string strEndDateInvoice { get; set; }
        public string strIsPPHFinal { get; set; }
    }
}