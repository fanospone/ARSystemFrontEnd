using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxInvoiceTowerProcess
    {
        public string SumADPP { get; set; }
        public string SumAPPN { get; set; }
        public string SumAPenalty { get; set; }
        public string SumATotalInvoice { get; set; }
        public List<ARSystemService.vwDataBAPSConfirm> ListTrxArDetail { get; set; }
        public string ReturnRemarks { get; set; }
        public string SumAPPH { get; set; }
        public bool IsPPN{ get; set; }
        public bool IsPPH { get; set; }
        public int PercentValue { get; set; }
        public List<ARSystemService.vwDataBAPSRemainingAmount> ListTrxArDetailRemaining { get; set; }
        public string SumADiscount { get; set; }
        public bool IsGR { get; set; }

    }
}