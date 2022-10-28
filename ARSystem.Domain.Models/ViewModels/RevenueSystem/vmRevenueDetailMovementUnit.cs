using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueDetailMovementUnit : BaseClass
    {
        public vmRevenueDetailMovementUnit()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmRevenueDetailMovementUnit(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string SoNumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string RegionName { get; set; }
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public DateTime? RFIDate { get; set; }
        public DateTime? SLDDate { get; set; }
        public DateTime? StartBapsDate { get; set; }
        public int PreviousMonth { get; set; }
        public int CurrentMonth { get; set; }
        public int Movement { get; set; }
    }
}
