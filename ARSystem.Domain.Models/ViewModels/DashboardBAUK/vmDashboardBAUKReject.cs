using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDashboardBAUKReject : BaseClass
    {
        public vmDashboardBAUKReject()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmDashboardBAUKReject(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
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
