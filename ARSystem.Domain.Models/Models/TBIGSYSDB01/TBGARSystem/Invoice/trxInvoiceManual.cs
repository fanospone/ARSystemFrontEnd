
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxInvoiceManual : BaseClass
	{
		public trxInvoiceManual()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxInvoiceManual(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public long trxInvoiceManualID { get; set; }
        public string SONumber { get; set; }
        public string SiteIDOpr { get; set; }
        public string SiteNameOpr { get; set; }
        public string InitialPONumber { get; set; }
        public DateTime InvoiceStartDate { get; set; }
        public DateTime InvoiceEndDate { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? OMPrice { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? AmountRounding { get; set; }
        public decimal? AmountPPN { get; set; }
        public decimal? AmountLossPPN { get; set; }
        public int? StipSiro { get; set; }
        public string CompanyID { get; set; }
        public string PriceCurrency { get; set; }
        public bool? LossPNN { get; set; }
        public bool? isPPHFinal { get; set; }
        public string BapsNO { get; set; }
        public string BapsType { get; set; }
        public string BapsPeriod { get; set; }
        public string OperatorName { get; set; }
        public DateTime? StartLeaseDate { get; set; }
        public DateTime? EndLeaseDate { get; set; }
        public string InvoiceTerm { get; set; }
        public string MLANumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? mstInvoiceStatusId { get; set; }
    }
}