using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmStringResult : BaseClass
    {
        public vmStringResult()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmStringResult(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string Result{ get; set; }
        public bool isShowPPH { get; set; }
        public bool isCreate15 { get; set; }
        public bool isReprint { get; set; }
        public double PPHValue { get; set; }
        public double PPFValue { get; set; }
        public bool isShowPPE { get; set; }
        public bool isShowPAT { get; set; }
        public bool isShowPPF { get; set; }
        public bool isLossPPN { get; set; }
        public string ApprovalStatus { get; set; }
        public int PrintCount { get; set; }

        public List<string> ListPostedInvoiceNumber { get; set; }
        public bool isValid { get; set; }
        public bool isPPHFinal { get; set; }
        public string Operator  { get; set; }
        public double PPNValue { get; set; }
        public string PPNText { get; set; }
    }
}
