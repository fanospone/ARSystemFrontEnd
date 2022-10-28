
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwDataCreatedInvoiceTower : BaseClass
	{
		public vwDataCreatedInvoiceTower()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDataCreatedInvoiceTower(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string InvTemp { get; set; }
		public string InvNo { get; set; }
		public int? trxInvoiceHeaderID { get; set; }
		public DateTime? InvDate { get; set; }
		public DateTime CreateInvoice { get; set; }
		public string InvoiceTypeId { get; set; }
		public string TermPeriod { get; set; }
		public string Company { get; set; }
		public string CompanyInvoice { get; set; }
		public string Operator { get; set; }
		public string OperatorInvoice { get; set; }
		public decimal? AmountADPP { get; set; }
		public decimal? Amount { get; set; }
		public decimal? Discount { get; set; }
		public decimal? InvTotalAPPN { get; set; }
		public decimal? InvTotalPenalty { get; set; }
		public string Currency { get; set; }
		public string CompanyName { get; set; }
		public string InvStatus { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public int? ReturnToInvStatus { get; set; }
		public string InvRemarksPrint { get; set; }
		public string InvRemarksPosting { get; set; }
		public bool? IsPPH { get; set; }
		public bool? IsPPN { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public int? mstPICATypeID { get; set; }
		public int? mstPICADetailID { get; set; }
        public bool? InvoiceManual { get; set; }
        public bool? InvoiceNonRevenue { get; set; }
    }
}