
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxReconcile : BaseClass
	{
		public trxReconcile()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxReconcile(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public int? mstBapsID { get; set; }
		public long? mstRAScheduleID { get; set; }
		public int? mstBAPSRecurringID { get; set; }
		public string BAPSNumber { get; set; }
		public string CustomerMLANumber { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
        public string CompanyInvoice { get; set; }
        public int? Year { get; set; }
		public int? Term { get; set; }
		public int? Quartal { get; set; }
		public decimal? ServicePrice { get; set; }
		public decimal? BaseLeasePrice { get; set; }
		public string ServiceCurrency { get; set; }
		public string BaseLeaseCurrency { get; set; }
		public DateTime? StartInvoiceDate { get; set; }
		public DateTime? EndInvoiceDate { get; set; }
		public int? mstRAActivityID { get; set; }
		public int? mstRABoqID { get; set; }
		public long? trxRAPurchaseOrderID { get; set; }
		public decimal? AmountIDR { get; set; }
		public decimal? AmountUSD { get; set; }
		public decimal? InflationAmount { get; set; }
		public string InflationCurrency { get; set; }
		public decimal? AdditionalAmount { get; set; }
		public string AdditionalCurrency { get; set; }
		public decimal? DeductionAmount { get; set; }
		public string DeductionCurrency { get; set; }
		public decimal? PenaltySlaAmount { get; set; }
		public string PenaltySlaCurrency { get; set; }
		public DateTime? ReconcileDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
        // For Update Date 20181002 - Ibnu Setiawan
        public DateTime? RFIDate { get; set; }
        public DateTime? BaufDate { get; set; }
    }
}