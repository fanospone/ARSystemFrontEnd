
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwStopAccrueSonumbList : BaseClass
	{
		public vwStopAccrueSonumbList()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwStopAccrueSonumbList(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;

        }
        public Int64 ViewIdx { get; set; }
        public Int64 RowIndex { get; set; }
        public string SONumber { get; set; }
		public string SiteName { get; set; }
		public string CompanyID { get; set; }
		public string SiteID { get; set; }
		public string CustomerID { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
		public DateTime? RFIDone { get; set; }
		public string SiteType { get; set; }
		public string SiteTypeID { get; set; }
		public int RegionID { get; set; }
		public string RegionName { get; set; }
		public int? ProductID { get; set; }
		public string Product { get; set; }
	}
}