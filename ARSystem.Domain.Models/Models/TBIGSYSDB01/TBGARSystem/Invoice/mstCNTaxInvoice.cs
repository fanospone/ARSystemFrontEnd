
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstCNTaxInvoice : BaseClass
	{
		public mstCNTaxInvoice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstCNTaxInvoice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int CNTaxInvoiceID { get; set; }
		public int TrxCNInvoiceHeaderID { get; set; }
		public int? trxCNInvoiceHeaderRemainingAmountId { get; set; }
		public string InvNo { get; set; }
		public string TaxInvoiceNo { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public int? trxCNInvoiceNonRevenueID { get; set; }

    }
}