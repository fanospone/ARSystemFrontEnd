using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDashboardBAUKActivity : BaseClass
    {
        public vmDashboardBAUKActivity()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmDashboardBAUKActivity(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string GroupSum { get; set; }
        public string GroupSumID { get; set; }
        public int RFIDone { get; set; }
        public decimal AmountRFIDone { get; set; }
        public int BAUKSubmitted { get; set; }
        public decimal AmountBAUKSubmitted { get; set; }
        public int BAUKApproved { get; set; }
        public decimal AmountBAUKApproved { get; set; }
        public int BAUKRejected { get; set; }
        public decimal AmountBAUKRejected { get; set; }
        public int Total { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal PercentTotal { get; set; }
    }
}
