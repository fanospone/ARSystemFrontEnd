
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCancelNoteFinance : BaseClass
	{
		public trxCancelNoteFinance()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCancelNoteFinance(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxCancelNoteFinanceID { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public int? trxCNInvoiceHeaderRemainingAmountID { get; set; }
		public int? trxInvoiceHeaderID { get; set; }
		public int? trxInvoiceHeaderRemainingAmountID { get; set; }
		public string CancelNoteNo { get; set; }
		public string MemoNo { get; set; }
		public DateTime? PrintDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public int? trxCNInvoiceNonRevenueID { get; set; }
        public int? trxInvoiceNonRevenueID { get; set; }
    }
}