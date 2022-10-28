
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxInvoicePayment : BaseClass
	{
		public trxInvoicePayment()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxInvoicePayment(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoicePaymentId { get; set; }
		public int mstPaymentCodeId { get; set; }
		public string VoucherNumber { get; set; }
		public int? trxInvoiceHeaderId { get; set; }
		public int? trxInvoiceHeaderRemainingAmountId { get; set; }
		public decimal? Amount { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? BatchNumber { get; set; }
		public DateTime? PaymentDate { get; set; }
		public int? mstPaymentBankID { get; set; }
		public bool? IsCollection { get; set; }
		public bool? IsAX { get; set; }
        public int? trxInvoiceNonRevenueID { get; set; }

    }
}