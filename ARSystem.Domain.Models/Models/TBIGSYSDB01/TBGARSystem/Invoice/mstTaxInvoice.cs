
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstTaxInvoice : BaseClass
	{
		public mstTaxInvoice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstTaxInvoice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int TaxInvoiceID { get; set; }
		public int TrxInvoiceHeaderID { get; set; }
		public int? trxInvoiceHeaderRemainingAmountId { get; set; }
        public int? trxInvoiceNonRevenueID { get; set; }
        public string InvNo { get; set; }
		public string TaxInvoiceNo { get; set; }
		public DateTime? TaxInvoiceDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}