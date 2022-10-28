
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCNInvoiceHeaderReject : BaseClass
	{
		public trxCNInvoiceHeaderReject()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCNInvoiceHeaderReject(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxCNInvoiceHeaderID { get; set; }
		public string InvNo { get; set; }
		public string InvTemp { get; set; }
		public string InvTaxNumber { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public string InvOperatorID { get; set; }
		public string InvOperatorAsset { get; set; }
		public string InvCompanyInvoice { get; set; }
		public string InvCompanyId { get; set; }
		public decimal? InvSumADPP { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public string InvSubject { get; set; }
		public DateTime? InvFirstPrintDate { get; set; }
		public string InvoiceTypeId { get; set; }
		public string InvOprRegionID { get; set; }
		public DateTime? InvReceiptDateByOperator { get; set; }
		public string InvInternalPIC { get; set; }
		public string InvExternalPIC { get; set; }
		public DateTime? InvPaidDate { get; set; }
		public decimal? InvAPaid { get; set; }
		public string InvPaidUser { get; set; }
		public string InvPaidStatus { get; set; }
		public decimal? InvTotalAPPH { get; set; }
		public decimal? InvTotalAPPN { get; set; }
		public int? InvIDPaymentBank { get; set; }
		public decimal? InvTotalPenalty { get; set; }
		public decimal? InvTotalDiscount { get; set; }
		public DateTime? InvReceiptDate { get; set; }
		public string InvReceiptFile { get; set; }
		public bool? InvIsAx { get; set; }
		public string Currency { get; set; }
		public string InvCollectionRemarks { get; set; }
		public string InvParentNo { get; set; }
		public string InvFARSignature { get; set; }
		public bool? InvIsParent { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public int? AppHeaderId { get; set; }
		public int? ReturnToInvStatus { get; set; }
		public string InvRemarksPrint { get; set; }
		public string InvRemarksPosting { get; set; }
		public bool? IsPPN { get; set; }
		public bool? IsPPH { get; set; }
		public bool? IsPPHFinal { get; set; }
		public string VerificationStatus { get; set; }
		public string VerificationRemark { get; set; }
		public DateTime? VerificationDate { get; set; }
		public string VerifiedBy { get; set; }
		public string ApprovalStatus { get; set; }
		public string ApprovalRemark { get; set; }
		public DateTime? ApprovalDate { get; set; }
		public string ApprovedBy { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? trxCNInvoiceHeaderParentId { get; set; }
		public string ARProcessRemark { get; set; }
		public string PICARemark { get; set; }
		public string ContentType { get; set; }
		public string FilePath { get; set; }
		public decimal? ARProcessPenalty { get; set; }
		public bool? InvoiceManual { get; set; }
	}
}