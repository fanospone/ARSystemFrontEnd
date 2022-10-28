
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxDocInvoiceDetail : BaseClass
	{
		public trxDocInvoiceDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxDocInvoiceDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxDocInvoiceDetailID { get; set; }
		public int trxInvoiceHeaderID { get; set; }
		public int DocInvoiceID { get; set; }
		public bool IsChecked { get; set; }
		public bool IsReceived { get; set; }
		public string Remark { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public int? trxInvoiceHeaderRemainingAmountId { get; set; }
        public int? trxInvoiceNonRevenueID { get; set; }
        public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}