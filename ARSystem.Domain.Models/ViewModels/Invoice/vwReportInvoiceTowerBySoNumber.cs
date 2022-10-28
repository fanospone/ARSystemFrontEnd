
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwReportInvoiceTowerBySoNumber : BaseClass
	{
		public vwReportInvoiceTowerBySoNumber()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReportInvoiceTowerBySoNumber(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoiceHeaderID { get; set; }
		public string InvCompanyId { get; set; }
		public string CompanyIdAx { get; set; }
		public string InvTemp { get; set; }
		public string Description { get; set; }
		public string InvoiceCategory { get; set; }
		public string InvOperatorID { get; set; }
		public string InvSubject { get; set; }
		public decimal? InvSumADPP { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public int Credit { get; set; }
		public string Currency { get; set; }
		public DateTime? StartDateRent { get; set; }
		public DateTime? EndDateRent { get; set; }
		public DateTime? StartDatePeriod { get; set; }
		public DateTime? EndDatePeriod { get; set; }
		public string SONumber { get; set; }
		public string SiteIdOld { get; set; }
		public string SiteName { get; set; }
		public DateTime? BapsDone { get; set; }
		public DateTime? BapsReceiveDate { get; set; }
		public string InvNo { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public DateTime? DueDate { get; set; }
		public string PostingProfile { get; set; }
		public string OffSetAccount { get; set; }
		public string TaxGroup { get; set; }
		public string TaxItemGroup { get; set; }
		public string TaxInvoiceNo { get; set; }
		public DateTime? BapsConfirmDate { get; set; }
		public DateTime? InvFirstPrintDate { get; set; }
		public DateTime? InvReceiptDate { get; set; }
		public int? LeadTimeVerificator { get; set; }
		public int? LeadTimeInputer { get; set; }
		public int? LeadTimeFinishing { get; set; }
		public int? LeadTimeARData { get; set; }
		public DateTime? FPJDate { get; set; }
		// Modification Or Added By Ibnu Setiawan 15. September 2020
        public string confirm_user_id { get; set; }
        public string confirm_user_name { get; set; }
        public string receipt_user_id { get; set; }
        public string receipt_user_name { get; set; }
        public string create_user_id { get; set; }
        public string create_user_name { get; set; }
        public string posting_inv_user_id { get; set; }
        public string posting_inv_user_name { get; set; }
        public string print_inv_user_id { get; set; }
        public string print_inv_user_name { get; set; }
        // Modification Or Added By Ibnu Setiawan 15. September 2020
	}
}