using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
using System;
    public class dwhDashboardPotentialRFITo1stBAPSBillingDetailModel : BaseClass
    {
		public dwhDashboardPotentialRFITo1stBAPSBillingDetailModel()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public dwhDashboardPotentialRFITo1stBAPSBillingDetailModel(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}

		public int ID { get; set; }
		public string POStep { get; set; }
		public string RFIStep { get; set; }
		public string BAUKStep { get; set; }
		public string BAPSStep { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string SoNumber { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
		public string CustomerID { get; set; }
		public string RegionName { get; set; }
		public string ProvinceName { get; set; }
		public string ResidenceName { get; set; }
		public string po_number { get; set; }
		public string MLANumber { get; set; }
		public DateTime? StartLeaseDate { get; set; }
		public DateTime? EndLeaseDate { get; set; }
		public string BapsType { get; set; }
		public string CustomerInvoice { get; set; }
		public string CompanyInvoice { get; set; }
		public string CompanyInvoiceName { get; set; }
		public string CompanyID { get; set; }
		public string Currency { get; set; }
		public DateTime? InvoiceStartDate { get; set; }
		public DateTime? InvoiceEndDate { get; set; }
		public decimal? BaseLeasePrice { get; set; }
		public string ServicePrice { get; set; }
		public string DeductionAmount { get; set; }
		public decimal? AmountTotal { get; set; }
		public string STIPCategory { get; set; }
		public DateTime? RFIDate { get; set; }
	}
}
