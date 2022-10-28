
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwtrxDataAccrueTemp : BaseClass
	{
		public vwtrxDataAccrueTemp()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwtrxDataAccrueTemp(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long trxDataAccrueID { get; set; }
		public string SONumber { get; set; }
		public int RegionID { get; set; }
		public string RegionName { get; set; }
		public DateTime EndDatePeriod { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteIDOpr { get; set; }
		public string SiteNameOpr { get; set; }
		public string CompanyID { get; set; }
		public string Company { get; set; }
		public string CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string Type { get; set; }
		public string SOW { get; set; }
		public string TypeSOW { get; set; }
		public int AccrueStatusID { get; set; }
		public string StatusAccrue { get; set; }
		public decimal? BaseLeasePrice { get; set; }
		public decimal? ServicePrice { get; set; }
		public decimal? TotalAmount { get; set; }
		public string Currency { get; set; }
		public DateTime? StartDateBAPS { get; set; }
		public DateTime? EndDateBAPS { get; set; }
		public DateTime? StartDateAccrue { get; set; }
		public DateTime? EndDateAccrue { get; set; }
		public string CompanyInvID { get; set; }
		public string StatusMasterList { get; set; }
		public string TenantType { get; set; }
		public DateTime? RFIDate { get; set; }
		public DateTime? SldDate { get; set; }
		public DateTime? BAPSDate { get; set; }
		public int? Month { get; set; }
		public int? D { get; set; }
		public string OD { get; set; }
		public string ODCategory { get; set; }
		public string MioAccrue { get; set; }
		public string Remarks { get; set; }
		public int? Week { get; set; }
		public int? WeekTargetUser { get; set; }
		public DateTime? TargetDateUser { get; set; }
		public int? RootCauseID { get; set; }
		public int? PicaID { get; set; }
		public int? PicaDetailID { get; set; }
		public string ContentTypeMove { get; set; }
		public string FilePathMove { get; set; }
		public string FileNameMove { get; set; }
		public string ContentTypeConfirm { get; set; }
		public string FilePathConfirm { get; set; }
		public string FileNameConfirm { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string KeyDataAccrue { get; set; }
	}
}