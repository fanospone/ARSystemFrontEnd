
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwReactiveStopAccrueDashboardDetail : BaseClass
	{
		public vwReactiveStopAccrueDashboardDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReactiveStopAccrueDashboardDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string RequestNumber { get; set; }
		public string Initiator { get; set; }
		public DateTime? StartEffectiveDate { get; set; }
		public DateTime? EndEffectiveDate { get; set; }
		public string AccrueType { get; set; }
		public long TrxStopAccrueDetailID { get; set; }
		public long? TrxStopAccrueHeaderID { get; set; }
		public string SONumber { get; set; }
		public int? CaseCategoryID { get; set; }
		public int? CaseDetailID { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public decimal? RevenueAmount { get; set; }
		public decimal? CapexAmount { get; set; }
		public decimal? CompensationAmount { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string Product { get; set; }
		public string CategoryCase { get; set; }
		public string DetailCase { get; set; }
		public DateTime? RFIDone { get; set; }
		public string Company { get; set; }
		public string Customer { get; set; }
		public string DepartName { get; set; }
		public bool? IsHold { get; set; }
		public string FileName { get; set; }
		public string DepartmentCode { get; set; }
		public DateTime? SubmissionDate { get; set; }
		public string Color { get; set; }
		public string DepartColor { get; set; }
	}
}