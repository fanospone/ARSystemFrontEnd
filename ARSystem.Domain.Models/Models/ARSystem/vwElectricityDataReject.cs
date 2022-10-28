
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwElectricityDataReject : BaseClass
	{
		public vwElectricityDataReject()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwElectricityDataReject(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string DT_RowId { get; set; }
		public int trxBapsRejectId { get; set; }
		public string SONumber { get; set; }
		public string SiteIdOld { get; set; }
		public string SiteIdOpr { get; set; }
		public string SiteName { get; set; }
		public string CompanyInvoice { get; set; }
		public string CompanyId { get; set; }
		public string Operator { get; set; }
		public string OperatorAsset { get; set; }
		public string Type { get; set; }
		public string BapsPeriod { get; set; }
		public string BapsNo { get; set; }
		public string PoNumber { get; set; }
		public string Status { get; set; }
		public string StatusDescription { get; set; }
		public string PeriodInvoice { get; set; }
		public decimal InvoiceAmount { get; set; }
		public decimal AmountRental { get; set; }
		public decimal AmountService { get; set; }
		public string InvoiceTypeId { get; set; }
		public string InvoiceTypeDesc { get; set; }
		public decimal AmountInvoicePeriod { get; set; }
		public string BapsType { get; set; }
		public string StipSiro { get; set; }
		public int StipSiroId { get; set; }
		public string PowerType { get; set; }
		public string PowerTypeCode { get; set; }
		public string Currency { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartDateInvoice { get; set; }
		public DateTime? EndDateInvoice { get; set; }
		public string Regional { get; set; }
		public bool? IsLossPPN { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string RejectRemarks { get; set; }
		public string Description { get; set; }
		public string PPHType { get; set; }
		public string ExpenseAdvance { get; set; }
		public string Advance { get; set; }
		public DateTime? RequestedDate { get; set; }
		public DateTime? ApproveFinDate { get; set; }
		public string ExpDescription { get; set; }
		public string COA { get; set; }
		public decimal? AmountExpense { get; set; }
		public string Employee { get; set; }
		public string DepartmentName { get; set; }
		public string DivisionName { get; set; }
		public string DirectorateName { get; set; }
		public string BankAccountNumber { get; set; }
		public string BankAccountName { get; set; }
		public string ReferenceNumber { get; set; }
		public string VOUCHER { get; set; }
		public DateTime TRANSDATE { get; set; }
		public DateTime? ElecPeriodeStart { get; set; }
		public DateTime? ElecPeriodeEnd { get; set; }
		public string BankName { get; set; }
		public string PICAStatus { get; set; }

        public int YearPeriod { get; set; }

    }
}