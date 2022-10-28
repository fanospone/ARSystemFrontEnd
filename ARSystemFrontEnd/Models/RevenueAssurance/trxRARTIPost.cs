using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class trxRARTIPost : DatatableAjaxModel
    {
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public string BAPS { get; set; }
        public List<string> strBAPSNumber { get; set; }
        public string PO { get; set; }
        public List<string> strPONumber { get; set; }
        public string SONumber { get; set; }
        public List<string> strSONumber { get; set; }
        public int isRaw { get; set; }
        public string Year { get; set; }
        public string Quartal { get; set; }
        public string BapsType { get; set; }
        public string PowerType { get; set; }
        public string trxReconcileID { get; set; }
        public bool IsOrder { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }

    }
}