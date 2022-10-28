using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxReportInvoiceTowerView : DatatableAjaxModel
    {
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }
        public int intYearPosting { get; set; }
        public int intMonthPosting { get; set; }
        public int intWeekPosting { get; set; }
        public string invNo { get; set; }
        public string strCompanyCode { get; set; }
    }
}