using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostAccountPayableSummary :  DatatableAjaxModel
    {
        public string strProduct { get; set; }
        public string strSoNumber { get; set; }
        public string strProductSite { get; set; }

        public string strQty { get; set; }
    }
}