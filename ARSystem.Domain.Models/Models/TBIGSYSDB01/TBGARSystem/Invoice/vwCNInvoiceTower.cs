
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwCNInvoiceTower : BaseClass
	{
		public vwCNInvoiceTower()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwCNInvoiceTower(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoiceHeaderID { get; set; }
		public string InvNo { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public string InvTemp { get; set; }
		public string Term { get; set; }
		public string Operator { get; set; }
		public string OperatorDesc { get; set; }
		public decimal? InvSumADPP { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public decimal? Discount { get; set; }
		public decimal? InvTotalAPPN { get; set; }
		public decimal? InvTotalAPPH { get; set; }
		public decimal? InvTotalPenalty { get; set; }
		public string Currency { get; set; }
		public DateTime? InvReceiptDate { get; set; }
		public string StatusReceipt { get; set; }
		public string InvoiceStatus { get; set; }
		public string InvReceiptFile { get; set; }
		public DateTime? ChecklistDate { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public string InvOperatorID { get; set; }
		public string InvCompanyId { get; set; }
		public string InvoiceTypeId { get; set; }
		public string InvoiceCategory { get; set; }
		public string InvInternalPIC { get; set; }
		public string ARProcessRemark { get; set; }
		public string FilePath { get; set; }
		public string ContentType { get; set; }
		public DateTime? PostingDate { get; set; }
		public string PostedBy { get; set; }
		public string TaxInvoiceNo { get; set; }
		public string CompanyTBG { get; set; }
		public int? mstPICATypeID { get; set; }
		public string PICAType { get; set; }
		public int? mstPICAMajorID { get; set; }
		public string PICAMajor { get; set; }
		public int? mstPICADetailID { get; set; }
		public string PICADetail { get; set; }
		public string Remark { get; set; }
		public int trxCNPICAARID { get; set; }
		public DateTime CNRequestDate { get; set; }
		public string ApprovalStatus { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string PicaTypeSec { get; set; }
		public string PicaDetailSec { get; set; }
		public DateTime? CNApprovalDate { get; set; }
        public string Source { get; set; }
	}
}