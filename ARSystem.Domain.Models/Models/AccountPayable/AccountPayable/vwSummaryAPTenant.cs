
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwSummaryAPTenant : BaseClass
	{
		public vwSummaryAPTenant()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwSummaryAPTenant(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SONumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string RegionalName { get; set; }
        public string StipCategory { get; set; }
        public string Product { get; set; }
		public decimal? TotalCost { get; set; }
		public decimal? TotalRevenue { get; set; }
		public long? RowIndex { get; set; }
	}
}