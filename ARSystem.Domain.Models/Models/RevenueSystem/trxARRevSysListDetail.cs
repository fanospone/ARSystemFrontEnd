
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxARRevSysListDetail : BaseClass
	{
		public trxARRevSysListDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxARRevSysListDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SONumber { get; set; }
		public int? DataPeriod { get; set; }
		public string DataMonth { get; set; }
		public int? DataMonthNumber { get; set; }
		public int? DataYear { get; set; }
		public DateTime? StartSLDDate { get; set; }
		public DateTime? EndSLDate { get; set; }
		public DateTime? StartBapsDate { get; set; }
		public DateTime? EndBapsDate { get; set; }
		public int? StipSiro { get; set; }
		public string StatusInvoice { get; set; }
		public DateTime? StartInvoiceDate { get; set; }
		public DateTime? EndInvoiceDate { get; set; }
		public string InvoiceNumber { get; set; }
		public decimal? TotalAmountInvoice { get; set; }
		public decimal? AmountPerMonth { get; set; }
		public decimal? AmountPerPeriod { get; set; }
		public decimal? BalanceAccrue { get; set; }
		public decimal? BalanceUnearned { get; set; }
		public decimal? TotalAdjustment { get; set; }

        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerID { get; set; }
        public string RegionName { get; set; }
        public string TenantType { get; set; }
        public string SiteStatus { get; set; }
        public decimal? TotalAccrued { get; set; }
        public decimal? TotalUnearned { get; set; }
    }
}