using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTimeLineActivity : DatatableAjaxModel
    {
        public string strCompanyID { get; set; }
        public string strCustomerID { get; set; }
        public string strUserID { get; set; }
        public string strTransactionID { get; set; }
        public string strActivity { get; set; }
        public string strDate { get; set; }
        public string strSONumber { get; set; }
    }
}