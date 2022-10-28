using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRevenueSoNumberList : BaseClass
    {
        public vmRevenueSoNumberList()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmRevenueSoNumberList(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string SoNumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerID { get; set; }
        public string RegionName { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public int? StipSiro { get; set; }
        public Int16? StipID { get; set; }
        public string StipCategory { get; set; }
        public decimal TotalAccrued { get; set; }
        public decimal TotalUnearned { get; set; }
        public decimal AmountRevenue { get; set; }
    }
}
