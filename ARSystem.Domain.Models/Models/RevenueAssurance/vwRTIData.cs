
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwRTIData : BaseClass
	{
		public vwRTIData()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRTIData(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long? Id { get; set; }
		public string SONumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
		public string CustomerID { get; set; }
		public string Company { get; set; }
		public string CompanyName { get; set; }
		public int? RegionID { get; set; }
		public string RegionName { get; set; }
		public int? ProvinceID { get; set; }
		public string ProvinceName { get; set; }
		public int? ResidenceID { get; set; }
		public string ResidenceName { get; set; }
		public int? Year { get; set; }
		public int? Term { get; set; }
		public int? Quartal { get; set; }
		public DateTime? StartInvoiceDate { get; set; }
		public DateTime? EndInvoiceDate { get; set; }
		public int? InvoiceTypeId { get; set; }
		public string InvoiceTypeName { get; set; }
		public DateTime? StartBapsDate { get; set; }
		public DateTime? EndBapsDate { get; set; }
		public int? StipSiro { get; set; }
		public long? POId { get; set; }
		public string PONumber { get; set; }
		public string BAPSNumber { get; set; }
		public DateTime? InitialPODate { get; set; }
		public decimal? BaseLeasePrice { get; set; }
		public decimal? ServicePrice { get; set; }
		public decimal? TotalPaymentRupiah { get; set; }
		public string BaseLeaseCurrency { get; set; }
		public string ServiceCurrency { get; set; }
		public string MLANumber { get; set; }
		public string TenantType { get; set; }
		public string CompanyInvoice { get; set; }
		public string CompanyInvoiceName { get; set; }
		public string CustomerInvoice { get; set; }
		public decimal? InflationAmount { get; set; }
		public string InflationCurrency { get; set; }
		public decimal? AdditionalAmount { get; set; }
		public string AdditionalCurrency { get; set; }
		public decimal? DeductionAmount { get; set; }
		public string DeductionCurrency { get; set; }
		public decimal? PenaltySlaAmount { get; set; }
		public string PenaltySlaCurrency { get; set; }
		public DateTime? ReconcileDate { get; set; }
		public string BapsType { get; set; }
		public string PowerType { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string ContentType { get; set; }
		public int? ProductID { get; set; }
		public decimal? DropFODistance { get; set; }
	}
}