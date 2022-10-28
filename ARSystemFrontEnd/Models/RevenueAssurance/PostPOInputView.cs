using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostPOInputView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strGroupBy { get; set; }
        public string strCurrency { get; set; }
        public string strBoqID { get; set; }
        public string strRegional { get; set; }
        public string strSiteID { get; set; }
        public string strSiteIDOpr { get; set; }
        public int isRaw { get; set; }
        public string trxRAPurchaseOrderID { get; set; }

        public string Activity { get; set; }
    }

    public class PostFilterReconcilePO : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strCustomerId { get; set; }
        public string strStipID { get; set; }
        public string strCurrency { get; set; }
        public string strSONumber { get; set; }
        public string strQuarterly { get; set; }
        public string strYear { get; set; }
        public string strProduct { get; set; }
        public string strBapsType { get; set; }
        public string strPowerType { get; set; }
        public string strSiteID { get; set; }
        public string strSiteName { get; set; }
        public string strFilterID { get; set; }

        public List<ARSystemService.vmStatus> param { get; set; }
    }
}