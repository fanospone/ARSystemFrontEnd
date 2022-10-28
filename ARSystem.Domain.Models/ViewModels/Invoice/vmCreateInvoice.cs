using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Domain.Models
{
    public class vmCreateInvoice : BaseClass
    {
        public vmCreateInvoice()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmCreateInvoice(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string SumADPP { get; set; }
        public string SumAPPN { get; set; }
        public string SumAPenalty { get; set; }
        public string SumATotalInvoice { get; set; }
        public string SumAPPH { get; set; }
        public bool IsPPN { get; set; }
        public bool IsPPH { get; set; }
        public List<vwDataBAPSConfirm> ListTrxArDetail { get; set; }
        public List<vwDataBAPSRemainingAmount> ListTrxArDetailRemaining { get; set; }
        public List<vwDataPostedInvoiceTower> ListInvoicePosted{ get; set; }
        public int PercentValue { get; set; }
        public string SumADiscount { get; set; }
        public bool? InvoiceManual { get; set; }
        public bool IsGR { get; set; }
    }
}
