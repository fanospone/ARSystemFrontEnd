
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwDataPostedInvoiceTower : BaseClass
	{
		public vwDataPostedInvoiceTower()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDataPostedInvoiceTower(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string DT_RowId { get; set; }
		public int trxInvoiceHeaderID { get; set; }
		public string InvNo { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public string InvTypeDesc { get; set; }
		public string InvCompanyId { get; set; }
		public string InvOperatorID { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public decimal? InvTotalAPPN { get; set; }
		public decimal? InvTotalPenalty { get; set; }
		public string Currency { get; set; }
		public int? InvPaidStatus { get; set; }
		public string CNStatus { get; set; }
		public string PrintStatus { get; set; }
        public string PrintStatusCover { get; set; } /* Edd By MTR */
        public string InvoiceTypeId { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public string BapsType { get; set; }
		public bool? IsLossPPN { get; set; }
		public string Company { get; set; }
        public string CompanyInvoiceID { get; set; }/* Add By ASE */
        public string CompanyInvoice { get; set; }/* Add By ASE */
        public string BankNameCompanyInvoice { get; set; }/* Add By ASE */
        public string AccNoCompanyInvoice { get; set; }/* Add By ASE */
        public string OperatorDesc { get; set; }
		public string Address { get; set; }
		public string Address3 { get; set; }
		public string Section { get; set; }
		public string InvSubject { get; set; }
		public decimal? InvSumADPP { get; set; }
		public string BankName { get; set; }
		public string AccNo { get; set; }
		public string FullName { get; set; }
		public string Position { get; set; }
		public int PrintCount { get; set; }
		public string InvStatus { get; set; }
		public string InvRemarksPrint { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public string Terbilang { get; set; }
		public string ChecklistStatus { get; set; }
		public string InvoiceCategory { get; set; }
		public decimal? InvTotalDiscount { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool? IsPPHFinal { get; set; }
		public string PPHType { get; set; }
		public string PostingProfile { get; set; }
		public string ApprovalStatus { get; set; }
		public string InvAdditionalNote { get; set; }
		public bool? IsPPH { get; set; }
		public string ReprintString { get; set; }
		public string PrintUsers { get; set; }
        public bool? InvoiceManual { get; set; }
    }
}