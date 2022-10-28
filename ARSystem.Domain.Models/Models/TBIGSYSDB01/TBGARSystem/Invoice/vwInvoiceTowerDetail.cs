
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwInvoiceTowerDetail : BaseClass
	{
		public vwInvoiceTowerDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwInvoiceTowerDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoiceHeaderID { get; set; }
		public string SONumber { get; set; }
		public string SiteIdOld { get; set; }
		public string SiteName { get; set; }
		public string PeriodNo { get; set; }
		public decimal? AmountRental { get; set; }
		public decimal? AmountService { get; set; }
		public int? DiscountRental { get; set; }
		public int? DiscountService { get; set; }
		public decimal? Total { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
	}
}