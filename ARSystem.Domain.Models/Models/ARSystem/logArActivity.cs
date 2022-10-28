
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class logArActivity : BaseClass
	{
		public logArActivity()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logArActivity(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int logArActivityId { get; set; }
		public int mstInvoiceStatusId { get; set; }
		public int? trxBapsDataId { get; set; }
		public int? trxArDetailId { get; set; }
		public int? trxArDetailRemainingAmountId { get; set; }
		public int? trxInvoiceHeaderID { get; set; }
		public int? trxInvoiceHeaderRemainingAmountID { get; set; }
		public int? LogWeek { get; set; }
		public int? PICAReprintID { get; set; }
		public string ReprintRemarks { get; set; }
		public int? BatchNumber { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
        public long? trxInvoiceManualID { get; set; }
        public bool IsCover { get; set; } /* Edd By MTR */
        public int? trxInvoiceNonRevenueID { get; set; } /* Add By NHF */
    }
}