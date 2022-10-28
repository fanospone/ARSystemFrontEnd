
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxInvoiceNonRevenue : BaseClass
	{
		public trxInvoiceNonRevenue()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxInvoiceNonRevenue(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public int trxInvoiceNonRevenueID { get; set; }
        public string InvNo { get; set; }
        public string InvTaxNumber { get; set; }
        public string CompanyID { get; set; }
        public string OperatorID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DPP { get; set; }
        public decimal? TotalPPN { get; set; }
        public decimal? TotalPPH { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? InvoiceTotal { get; set; }
        public bool IsPPN { get; set; }
        public bool? IsPPH { get; set; }
        public int? mstInvoiceCategoryId { get; set; }
        public int? mstInvoiceStatusId { get; set; }
        public string InvoiceTypeId { get; set; }
        public DateTime? InvPrintDate { get; set; }
        public string InvSubject { get; set; }
        public string InvOprRegionID { get; set; }
        public string InvFARSignature { get; set; }
        public string InvAdditionalNote { get; set; }
        public string VerificationStatus { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string InvPaidStatus { get; set; }
        public string Currency { get; set; }
        public int? ReturnToInvStatus { get; set; }
        public string InvRemarksPosting { get; set; }
        public DateTime? InvFirstPrintDate { get; set; }
        public string InvRemarksPrint { get; set; }
        public DateTime? InvReceiptDate { get; set; }
        public string InvReceiptFile { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public string ARProcessRemark { get; set; }
        public string InvInternalPIC { get; set; }
        public decimal? ARProcessPenalty { get; set; }
        public DateTime? InvPaidDate { get; set; }
        public int? InvIDPaymentBank { get; set; }
        public decimal? InvAPaid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        //add NHF 16 Feb 22
        public bool? IsPPHFinal { get; set; }
        public int? mstCategoryInvoiceID { get; set; }

    }
}