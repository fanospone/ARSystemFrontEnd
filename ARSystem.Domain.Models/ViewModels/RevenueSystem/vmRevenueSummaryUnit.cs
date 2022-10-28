using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueSummaryUnit : BaseClass
    {
        public vmRevenueSummaryUnit()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmRevenueSummaryUnit(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string Description { get; set; }
        public double Jan { get; set; }
        public double Feb { get; set; }
        public double Mar { get; set; }
        public double Apr { get; set; }
        public double May { get; set; }
        public double Jun { get; set; }
        public double Jul { get; set; }
        public double Aug { get; set; }
        public double Sep { get; set; }
        public double Oct { get; set; }
        public double Nov { get; set; }
        public double Dec { get; set; }
        public double Total { get; set; }
        public string TotalInPercent { get; set; }
    }
}
