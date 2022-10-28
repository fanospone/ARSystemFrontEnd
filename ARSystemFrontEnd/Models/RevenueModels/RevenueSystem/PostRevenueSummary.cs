using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostRevenueSummary : DatatableAjaxModel
    {
        public string fSoNumber { get; set; }
        public int fStipSiro { get; set; }
        public string fAccount { get; set; }
        public int? fYear { get; set; }
        public int? fMonth { get; set; }
        public string fCompanyId { get; set; }
        public string fRegionalName { get; set; }
        public string fOperatorId { get; set; }
        public string fProduct { get; set; }
        public string fGroupBy { get; set; }
        public string fGroupValue { get; set; }
        public string fViewBy { get; set; }
        public string schSONumber { get; set; }
        public string schSiteName { get; set; }
    }
}