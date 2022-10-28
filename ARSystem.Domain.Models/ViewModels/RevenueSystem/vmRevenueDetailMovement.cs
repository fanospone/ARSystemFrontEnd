using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueDetailMovement : BaseClass
    {
        public vmRevenueDetailMovement()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmRevenueDetailMovement(int errorType, string errorMessage)
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
        public decimal PreviousMonth { get; set; }
        public decimal CurrentMonth { get; set; }
        public decimal Movement { get; set; }
    }
}
