using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDashboardTSELOverdue : BaseClass
    {
        public vmDashboardTSELOverdue()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmDashboardTSELOverdue(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string Type { get; set; }
        public string ValueString { get; set; }
        public int Value { get; set; }
        public decimal Percentage { get; set; }
        public int CountSite { get; set; }
    }
}
