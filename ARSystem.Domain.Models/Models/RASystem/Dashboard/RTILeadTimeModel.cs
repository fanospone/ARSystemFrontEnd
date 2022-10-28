using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class RTILeadTimeModel : BaseClass
    {
        public RTILeadTimeModel()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public RTILeadTimeModel(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }


        public string CustomerID  { get; set; }
        public string Category { get; set; }
        public int CountSite { get; set; }
        public string CurrentStatus { get; set; }
        public decimal Everage { get; set; }
    }


}
