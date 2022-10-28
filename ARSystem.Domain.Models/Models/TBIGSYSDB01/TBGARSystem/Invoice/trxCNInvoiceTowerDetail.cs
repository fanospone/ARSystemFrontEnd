
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCNInvoiceTowerDetail : BaseClass
	{
		public trxCNInvoiceTowerDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCNInvoiceTowerDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxCNInvoiceTowerDetailId { get; set; }
		public int trxCNInvoiceHeaderID { get; set; }
		public string SONumber { get; set; }
		public string SiteIdOld { get; set; }
		public string SiteName { get; set; }
		public DateTime? SpkDate { get; set; }
		public DateTime? RfiDate { get; set; }
		public string BapsPeriod { get; set; }
		public string PoNumber { get; set; }
		public string Type { get; set; }
		public string BapsNo { get; set; }
		public string PeriodInvoice { get; set; }
		public string BapsType { get; set; }
		public string StipSiro { get; set; }
		public int StipSiroId { get; set; }
		public string PowerType { get; set; }
		public string PowerTypeCode { get; set; }
		public DateTime? StartDatePeriod { get; set; }
		public DateTime? EndDatePeriod { get; set; }
		public decimal AmountRental { get; set; }
		public decimal AmountService { get; set; }
		public decimal AmountInvoicePeriod { get; set; }
		public decimal AmountPenaltyPeriod { get; set; }
		public decimal AmountOverdaya { get; set; }
		public decimal AmountOverblast { get; set; }
		public decimal? AmountPPN { get; set; }
		public decimal? AmountLossPPN { get; set; }
		public bool? IsLossPPN { get; set; }
		public bool? IsPartial { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}