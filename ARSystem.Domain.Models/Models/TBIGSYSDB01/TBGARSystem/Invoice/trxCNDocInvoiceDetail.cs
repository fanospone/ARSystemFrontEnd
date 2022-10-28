
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCNDocInvoiceDetail : BaseClass
	{
		public trxCNDocInvoiceDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCNDocInvoiceDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxCNDocInvoiceDetailID { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public int? trxCNInvoiceHeaderRemainingAmountId { get; set; }
		public int DocInvoiceID { get; set; }
		public bool IsChecked { get; set; }
		public bool IsReceived { get; set; }
		public string Remark { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public int? trxCNInvoiceNonRevenueID { get; set; }

    }
}