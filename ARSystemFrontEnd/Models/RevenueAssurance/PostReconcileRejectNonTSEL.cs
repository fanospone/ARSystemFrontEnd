using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReconcileRejectNonTSEL : DatatableAjaxModel
    {

        public string strCustomerID { get; set; }
        public string strCompanyID { get; set; }
        public int strBAPSTypeID { get; set; }
        public int strSTIPID { get; set; }
        public string strSONumber { get; set; }
        public int strQuarter { get; set; }
        public int strYear { get; set; }
        public string strSiteID { get; set; }
        public int strProductID { get; set; }
        public int strPowerTypeID { get; set; }
        public int strReconcileID { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public string strBaseLease { get; set; }
        public string strServicePrice { get; set; }
        public string strInvoiceTypeID { get; set; }
        public string strBaseLeaseCurr { get; set; }
        public string strServiceCurr { get; set; }
        public string strPOType { get; set; }
        public int strPODtlID { get; set; }

        public ARSystemService.trxReconcile trxReconcile = new ARSystemService.trxReconcile();
        public ARSystemService.vwRAReconcileRejectNonTSEL vwReconcile = new ARSystemService.vwRAReconcileRejectNonTSEL();
    }
}