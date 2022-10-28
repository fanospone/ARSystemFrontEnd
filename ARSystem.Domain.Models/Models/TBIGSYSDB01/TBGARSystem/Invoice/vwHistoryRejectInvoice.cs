
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwHistoryRejectInvoice : BaseClass
	{
		public vwHistoryRejectInvoice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwHistoryRejectInvoice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string Source { get; set; }
		public string SONumber { get; set; }
		public string SiteIdOld { get; set; }
		public string SiteName { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
		public string CustomerInvoice { get; set; }
		public string CompanyInvoice { get; set; }
		public DateTime? StartDateInvoice { get; set; }
		public DateTime? EndDateInvoice { get; set; }
		public decimal? AmountRental { get; set; }
		public decimal? AmountService { get; set; }
		public string BaseLeaseCurrency { get; set; }
		public string ServiceCurrency { get; set; }
		public int? AmountIDR { get; set; }
		public int? AmountUSD { get; set; }
		public int? trxCNInvoiceTowerDetailID { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public string Product { get; set; }
		public string BapsType { get; set; }
		public string PowerType { get; set; }
		public string StipSiro { get; set; }
		public string BapsNo { get; set; }
		public string InvNo { get; set; }
		public string ReconcileType { get; set; }
		public string DepartmentCode { get; set; }
		public string DepartmentName { get; set; }
		public int? RejectYear { get; set; }
		public int? RejectMonth { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public string PicaType { get; set; }
		public string PicaMajor { get; set; }
		public string PicaDetail { get; set; }
	}
}