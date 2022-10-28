
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class dwhDashboardRAPotentialSIRODetailModel : BaseClass
    {
		public dwhDashboardRAPotentialSIRODetailModel()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public dwhDashboardRAPotentialSIRODetailModel(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public long? mstRAScheduleID { get; set; }
		public string SoNumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
		public string CustomerID { get; set; }
		public int? RegionID { get; set; }
		public string RegionName { get; set; }
		public string ProvinceName { get; set; }
		public string ResidenceName { get; set; }
		public DateTime? RFIDate { get; set; }
		public DateTime? FirstBAPSDone { get; set; }
		public int? STIPID { get; set; }
		public string StipCategory { get; set; }
		public int? StipSiro { get; set; }
		public DateTime? StipDate { get; set; }
		public string PONumber { get; set; }
		public string MLANumber { get; set; }
		public DateTime? StartBapsDate { get; set; }
		public DateTime? EndBapsDate { get; set; }
		public int? Term { get; set; }
		public int? mstBapsTypeID { get; set; }
		public string BapsType { get; set; }
		public string CustomerInvoice { get; set; }
		public string CompanyInvoice { get; set; }
		public string Company { get; set; }
		public string Currency { get; set; }
		public DateTime? StartInvoiceDate { get; set; }
		public DateTime? EndInvoiceDate { get; set; }
		public int? StartInvoiceYear { get; set; }
		public string YearCategory { get; set; }
		public decimal? BaseLeasePrice { get; set; }
		public decimal? ServicePrice { get; set; }
		public decimal? DeductionAmount { get; set; }
		public decimal? TotalAmount { get; set; }
		public int? ProductID { get; set; }
		public string ProductName { get; set; }
		public int? SectionProductId { get; set; }
		public string SectionName { get; set; }
		public int? SowProductId { get; set; }
		public string SowName { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
