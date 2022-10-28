using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostRevenuePerSonumb : DatatableAjaxModel
    {
        public string strAccount { get; set; }
        public string strPeriodeTo { get; set; }
        public string strPeriode { get; set; }
        public string strCompany { get; set; }
        public string strRegional { get; set; }
        public string strOperator { get; set; }
        public string strProduct { get; set; }
        public string schSoNumber { get; set; }

    }
}   