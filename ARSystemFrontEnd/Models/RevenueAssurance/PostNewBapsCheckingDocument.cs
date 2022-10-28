using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostNewBapsCheckingDocument : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strGroupBy { get; set; }
        public string strProductId { get; set; }
        public string strBAPSType { get; set; }
        public string strCustomerID { get; set; }
        public string strSoNumber { get; set; }
        public string strSiteName { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public string strSiteID { get; set; }
        public string strTenantType { get; set; }
        public string strDocID { get; set; }
        public string strBapsChecked { get; set; }
        public string strAction { get; set; }
        public string mstRAActivityID { get; set; }
        public string Remarks { get; set; }
        public string vRemarks { get; set; }
        public string vAction { get; set; }
        public string vPICReceive { get; set; }
        public DateTime? vReceiveDate { get; set; }
        public string vSiteName { get; set; }
        public PostDocumentCheck[] DocumentCheck { get; set; }
        public string strStipID { get; set; }
        public string strSiroID { get; set; }
        public DateTime? strStartBaukDoneDate { get; set; }
        public DateTime? strEndBaukDoneDate { get; set; }
        public List<string> strSONumberMultiple { get; set; }

    }

    public class PostDocumentCheck
    {
        public int DocumentId { get; set; }
        public string SoNumber { get; set; }
        public byte CheckListType { get; set; }
        public string CheckListName { get; set; }
        public string Remark { get; set; }
    }

    public class PostDocumentArchieve : DatatableAjaxModel
    {
        public string DoneDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public string SoNumber { get; set; }
        public string TowerTypeID { get; set; }
    }
}