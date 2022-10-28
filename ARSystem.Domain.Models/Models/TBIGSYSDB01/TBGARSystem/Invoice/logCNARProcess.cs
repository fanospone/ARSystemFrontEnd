
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class logCNARProcess : BaseClass
	{
		public logCNARProcess()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logCNARProcess(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int logCNARProcessID { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public int? trxCNInvoiceHeaderRemainingAmountID { get; set; }
		public decimal? ARProcessPenalty { get; set; }
		public string InvInternalPIC { get; set; }
		public DateTime? ReceiptDate { get; set; }
		public string InvReceiptFile { get; set; }
		public string ContentType { get; set; }
		public string FilePath { get; set; }
		public string Remark { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public int? trxCNInvoiceNonRevenueID { get; set; }

    }
}