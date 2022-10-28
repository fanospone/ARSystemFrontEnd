
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class logARProcess : BaseClass
	{
		public logARProcess()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logARProcess(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int logARProcessID { get; set; }
		public int? trxInvoiceHeaderID { get; set; }
		public int? trxInvoiceHeaderRemainingAmountID { get; set; }
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
        public int? trxInvoiceNonRevenueID { get; set; }

    }
}