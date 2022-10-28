using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.ARSystem.ARRMonitoringCashIn
{
    class vmFilters : BaseClass
    {
        public vmFilters()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmFilters(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
       
        public string Name { get; set; }
        public int IntValue { get; set; }
        public string StrValue  { get; set; }
    }
}
