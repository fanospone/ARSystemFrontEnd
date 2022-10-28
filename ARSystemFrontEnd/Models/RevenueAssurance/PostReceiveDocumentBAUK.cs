using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReceiveDocumentBAUK : DatatableAjaxModel
    {
        public string vSONumber { get; set; }
        public string vSiteID { get; set; }
        public string vSiteName { get; set; }
        public DateTime? vStartSubmit { get; set; }
        public DateTime? vEndSubmit { get; set; }
        public string vCustomerID { get; set; }
        public string vCompanyID { get; set; }
        public string vStatusDoc { get; set; }
        public int vProductID { get; set; }
        public int vStip { get; set; }
        public List<string> strSONumber { get; set; }
        public string vAction { get; set; }
        public string vPICReceive { get; set; }
        public DateTime? vReceiveDate { get; set; }
        public List<trxReceiveDocumentBAUK> ListTrxReceive { get; set; }
        public string vRemarks { get; set; }
    }
}