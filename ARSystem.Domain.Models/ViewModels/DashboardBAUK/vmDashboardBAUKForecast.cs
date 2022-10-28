using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDashboardBAUKForecast : BaseClass
    {
        public vmDashboardBAUKForecast()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmDashboardBAUKForecast(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string GroupSum { get; set; }
        public string GroupSumID { get; set; }
        public int TotOutstanding { get; set; }
        public decimal AmountTotOutstanding { get; set; }
        public int TotW1 { get; set; }
        public decimal AmountTotW1 { get; set; }
        public int TotW2 { get; set; }
        public decimal AmountTotW2 { get; set; }
        public int TotW3 { get; set; }
        public decimal AmountTotW3 { get; set; }
        public int TotW4 { get; set; }
        public decimal AmountTotW4 { get; set; }
        public int TotW5 { get; set; }
        public decimal AmountTotW5 { get; set; }
        public int TotM1 { get; set; }
        public decimal AmountTotM1 { get; set; }
        public int TotM2 { get; set; }
        public decimal AmountTotM2 { get; set; }
        public int TotM3 { get; set; }
        public decimal AmountTotM3 { get; set; }
        public int TotM4 { get; set; }
        public decimal AmountTotM4 { get; set; }
        public int TotM5 { get; set; }
        public decimal AmountTotM5 { get; set; }
        public int TotNA { get; set; }
        public decimal AmountTotNA { get; set; }
    }
}
