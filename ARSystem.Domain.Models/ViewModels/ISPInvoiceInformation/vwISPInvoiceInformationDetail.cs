
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwISPInvoiceInformationDetail : BaseClass
	{
		public vwISPInvoiceInformationDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwISPInvoiceInformationDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public string SONumber { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string TenantType { get; set; }
        public DateTime? RFIDateOpr { get; set; }
        public int? StipSiro { get; set; }
        public DateTime? StartInvoice { get; set; }
        public DateTime? EndInvoice { get; set; }
        public string RAACtivityName { get; set; }
        public string PoNumber { get; set; }
        public string DocPO { get; set; }
        public string DocType { get; set; }
        public string BapsNo { get; set; }
        public DateTime? BAPSConfirmDate { get; set; }
        public string DocBAPS { get; set; }
        public decimal? BasicPrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? AmountInvoice { get; set; }
        public string NoInvoice { get; set; }
        public string InvTaxNumber { get; set; }
        public DateTime? CreateDateInvoice { get; set; }
        public DateTime? PostingDate { get; set; }
        public string PaidStatus { get; set; }
        public DateTime? PaidDate { get; set; }

    }
}