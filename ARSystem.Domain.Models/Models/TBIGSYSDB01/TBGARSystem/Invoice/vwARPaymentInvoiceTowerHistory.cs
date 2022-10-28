
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwARPaymentInvoiceTowerHistory : BaseClass
	{
		public vwARPaymentInvoiceTowerHistory()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwARPaymentInvoiceTowerHistory(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int? trxInvoiceHeaderID { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public string BatchCode { get; set; }
		public string PaymentDate { get; set; }
		public decimal? DBT { get; set; }
		public decimal? PAM { get; set; }
		public decimal? PNT { get; set; }
		public decimal? PPE { get; set; }
		public decimal? PPH { get; set; }
		public decimal? RTG { get; set; }
		public decimal? RND { get; set; }
		public decimal? PAT { get; set; }
		public decimal? PPF { get; set; }
		public decimal? Total { get; set; }
	}
}