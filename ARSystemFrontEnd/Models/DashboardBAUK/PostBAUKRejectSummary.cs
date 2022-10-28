using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAUKRejectSummary : DatatableAjaxModel
    {
        public string GroupSum { get; set; }
        public string GroupSumID { get; set; }
        public int TotReject { get; set; }
        public int TotImproper { get; set; }
        public int TotUncompleted { get; set; }
        public int TotWrong { get; set; }
        public int TotOther { get; set; }
        public int Total { get; set; }
        public decimal PercentTotal { get; set; }
    }
}