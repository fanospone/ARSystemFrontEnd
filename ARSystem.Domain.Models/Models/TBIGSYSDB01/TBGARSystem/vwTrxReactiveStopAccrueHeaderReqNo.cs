
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwTrxReactiveStopAccrueHeaderReqNo : BaseClass
	{
		public vwTrxReactiveStopAccrueHeaderReqNo()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwTrxReactiveStopAccrueHeaderReqNo(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long? RowIndex { get; set; }
		public string Initiator { get; set; }
		public string DeptCode { get; set; }
		public long? TrxStopAccrueHeaderID { get; set; }
		public string RequestNumber { get; set; }
		public int? RequestTypeID { get; set; }
		public string SONumber { get; set; }
		public int? CategoryCase { get; set; }
		public string DetailCase { get; set; }
		public int? CaseCategoryID { get; set; }
		public int? CaseDetailID { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public DateTime? StartEffectiveDate { get; set; }
		public DateTime? EndEffectiveDate { get; set; }
		public decimal? RevenueAmount { get; set; }
		public decimal? CapexAmount { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string Remarks { get; set; }
		public string FileName { get; set; }
		public long ID { get; set; }
		public string DepartInitial { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string CustomerSiteName { get; set; }
		public string Customer { get; set; }
		public DateTime? RFIDone { get; set; }
		public string Company { get; set; }
		public string Product { get; set; }
		public long? ViewIdx { get; set; }
		public string SubmissionType { get; set; }
	}
}