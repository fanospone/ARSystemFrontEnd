
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxBapsData : BaseClass
	{
		public trxBapsData()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxBapsData(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxBapsDataId { get; set; }
		public string SONumber { get; set; }
		public string SiteIdOld { get; set; }
		public string SiteIdOpr { get; set; }
		public string SiteName { get; set; }
		public string CompanyInvoice { get; set; }
		public string CompanyId { get; set; }
		public string Operator { get; set; }
		public string OperatorAsset { get; set; }
		public string Type { get; set; }
		public DateTime? SpkDate { get; set; }
		public DateTime? RfiDate { get; set; }
		public string BapsPeriod { get; set; }
		public string BapsNo { get; set; }
		public string PoNumber { get; set; }
		public string Status { get; set; }
		public string StatusDescription { get; set; }
		public string PeriodInvoice { get; set; }
		public decimal? InvoiceAmount { get; set; }
		public decimal? AmountRental { get; set; }
		public decimal? AmountService { get; set; }
		public string InvoiceTypeId { get; set; }
		public string InvoiceTypeDesc { get; set; }
		public decimal? AmountInvoicePeriod { get; set; }
		public decimal? AmountPenaltyPeriod { get; set; }
		public decimal? AmountOverdaya { get; set; }
		public decimal? AmountOverblast { get; set; }
		public string BapsType { get; set; }
		public string StipSiro { get; set; }
		public int? StipSiroId { get; set; }
		public string BapsDone { get; set; }
		public string PowerType { get; set; }
		public string PowerTypeCode { get; set; }
		public string Currency { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartDateInvoice { get; set; }
		public DateTime? EndDateInvoice { get; set; }
		public string SPK { get; set; }
		public string Kontrak { get; set; }
		public string Rekon { get; set; }
		public string BAPS { get; set; }
		public string BAK { get; set; }
		public string SSR { get; set; }
		public string PO { get; set; }
		public string BAUF { get; set; }
		public string Regional { get; set; }
		public bool? IsLossPPN { get; set; }
		public bool? IsPartial { get; set; }
		public decimal? AmountPPN { get; set; }
		public decimal? AmountLossPPN { get; set; }
		public decimal? AmountRounding { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public bool? IsPPHFinal { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? PPHFinalValidateDate { get; set; }
		public string SiteNameOpr { get; set; }
		public string ContractNumber { get; set; }
		public string ReslipNumber { get; set; }
		public int trxReconcileID { get; set; }
        public int? mstInvoiceStatusIdPOConfirm { get; set; }
        public string RemarksRejectPO { get; set; }
    }
}