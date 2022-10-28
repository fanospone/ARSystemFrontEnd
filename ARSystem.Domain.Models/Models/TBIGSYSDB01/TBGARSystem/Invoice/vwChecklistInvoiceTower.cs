
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwChecklistInvoiceTower : BaseClass
	{
		public vwChecklistInvoiceTower()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwChecklistInvoiceTower(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoiceHeaderID { get; set; }
		public string InvNo { get; set; }
		public string InvTemp { get; set; }
		public DateTime? InvPrintDate { get; set; }
		public string PostedBy { get; set; }
		public DateTime? PostedDate { get; set; }
		public decimal? InvSumADPP { get; set; }
		public decimal? InvTotalAmount { get; set; }
		public decimal? InvTotalAPPN { get; set; }
		public decimal? InvTotalAPPH { get; set; }
		public decimal? InvTotalPenalty { get; set; }
		public decimal? Discount { get; set; }
		public string Currency { get; set; }
		public string TaxInvoiceNo { get; set; }
		public string Remark { get; set; }
		public DateTime? VerificationDate { get; set; }
		public string Term { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public string Status { get; set; }
		public string VerificationStatus { get; set; }
		public string VerifiedBy { get; set; }
		public string InvCompanyId { get; set; }
		public string Company { get; set; }
		public string Operator { get; set; }
		public string InvOperatorID { get; set; }
		public string InvoiceTypeId { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? ChecklistARData { get; set; }
		public string PicaRemark { get; set; }
	}
}