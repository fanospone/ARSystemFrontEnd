using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDashboardBAUKFilter : BaseClass
    {
        public vmDashboardBAUKFilter()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmDashboardBAUKFilter(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public List<string> CustomerIDs{ get; set; }
        public List<string> CompanyIDs { get; set; }
        public List<int> STIPIDs { get; set; }
        public List<int> ProductIDs { get; set; }
        public List<int> Months { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string GroupBy { get; set; }
        public string strCustomer { get; set; }
        public string strCompany { get; set; }
        public string strSTIP { get; set; }
        public string strProduct { get; set; }
        public string strMonth { get; set; }
        public bool AmountMode { get; set; }
    }
}
