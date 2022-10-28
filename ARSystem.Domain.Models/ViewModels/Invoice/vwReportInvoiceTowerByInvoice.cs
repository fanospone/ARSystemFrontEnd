
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwReportInvoiceTowerByInvoice : BaseClass
	{
		public vwReportInvoiceTowerByInvoice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReportInvoiceTowerByInvoice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string DT_RowId { get; set; }
		public int trxInvoiceHeaderID { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public string InvCompanyId { get; set; }
		public string CompanyIdAx { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public string InvTemp { get; set; }
		public string AccountType { get; set; }
		public string InvOperatorID { get; set; }
		public string InvSubject { get; set; }
		public decimal? InvSumADPP { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public int Credit { get; set; }
		public string Currency { get; set; }
		public int Xrate { get; set; }
		public string SONumber { get; set; }
		public string DocNumber { get; set; }
		public DateTime? DueDate { get; set; }
		public string InvNo { get; set; }
		public string PostingProfile { get; set; }
		public string OffSetAccount { get; set; }
		public string TaxGroup { get; set; }
		public string TaxItemGroup { get; set; }
		public string TaxInvoiceNo { get; set; }
		public DateTime? PostingDate { get; set; }
		public string OperatorRegion { get; set; }
		public string Address { get; set; }
		public int? ReNo { get; set; }
		public int? ReceiptDate { get; set; }
		public DateTime? AfDate { get; set; }
		public string Status { get; set; }
		public string ElectricityCategory { get; set; }
		public string InvoiceCategory { get; set; }
		public DateTime? FPJDate { get; set; }
		public int isCNInvoice { get; set; }
	}
}