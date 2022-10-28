
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Domain.Models
{
	public class trxARCashInActualDetails : BaseClass
	{
		public trxARCashInActualDetails()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxARCashInActualDetails(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string InvoiceNo { get; set; }
		public DateTime? InvoicePrintDate { get; set; }
		public string InvoiceOperatorID { get; set; }
		public string InvoiceCompanyInvoice { get; set; }
		public decimal? InvoiceTotalAmount { get; set; }
		public string InvoiceSubject { get; set; }
		public string InvoiceTypeID { get; set; }
		public DateTime? InvoiceSubmitChecklistDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CretaedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? Month { get; set; }
		public int? MonthInQuarter { get; set; }
		public int? Quarter { get; set; }
		public int? Year { get; set; }
		public int? Week { get; set; }
	}
}