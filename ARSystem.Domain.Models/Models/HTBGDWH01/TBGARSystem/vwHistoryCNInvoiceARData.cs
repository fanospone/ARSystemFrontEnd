
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.HTBGDWH01.TBGARSystem
{
	public class vwHistoryCNInvoiceARData : BaseClass
	{
		public vwHistoryCNInvoiceARData()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwHistoryCNInvoiceARData(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string CNInfo { get; set; }
		public string CNInfoCode { get; set; }
		public string CNFrom { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public string InvCompanyId { get; set; }
		public string InvCompanyInvoice { get; set; }
		public decimal? ARProcessPenalty { get; set; }
		public string ARProcessRemark { get; set; }
		public decimal? InvSumADPP { get; set; }
		public string FilePath { get; set; }
		public string ContentType { get; set; }
		public DateTime? InvReceiptDate { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public string InvNo { get; set; }
		public string TaxNumber { get; set; }
		public string ReplacementStatus { get; set; }
		public DateTime? ReplaceDate { get; set; }
		public string ReplaceInvoice { get; set; }
		public string InvParentNo { get; set; }
		public string InvTemp { get; set; }
		public string InvoiceTypeId { get; set; }
		public string Description { get; set; }
		public string InvSubject { get; set; }
		public string InvOperatorID { get; set; }
		public string InvOperatorAsset { get; set; }
		public string InvOprRegionID { get; set; }
		public decimal? InvTotalAPPN { get; set; }
		public decimal? InvTotalAPPH { get; set; }
		public decimal? InvTotalDiscount { get; set; }
		public decimal? InvTotalPenalty { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public string Remark { get; set; }
		public string PicaTypeRequestor { get; set; }
		public string PicaDetailRequestor { get; set; }
		public string PicaTypeSec { get; set; }
		public string PicaDetailSec { get; set; }
		public string RequestedBy { get; set; }
		public DateTime RequestedDate { get; set; }
		public string ApprovedBy { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string SONumber { get; set; }
		public string isCNApproved { get; set; }
	}
}