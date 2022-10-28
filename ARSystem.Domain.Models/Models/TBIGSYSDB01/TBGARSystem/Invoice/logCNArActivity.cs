
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class logCNArActivity : BaseClass
	{
		public logCNArActivity()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logCNArActivity(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int logCNArActivityId { get; set; }
		public int mstInvoiceStatusId { get; set; }
		public int? trxBapsDataId { get; set; }
		public int? trxCNArDetailId { get; set; }
		public int? trxCNArDetailRemainingAmountId { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public int? trxCNInvoiceHeaderRemainingAmountID { get; set; }
		public int? LogWeek { get; set; }
		public int? PICAReprintID { get; set; }
		public string ReprintRemarks { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
        public long? trxInvoiceManualID { get; set; }
        public int? trxCNInvoiceNonRevenueID { get; set; }
    }
}