
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwDataInvoiceNonRevenue : BaseClass
	{
		public vwDataInvoiceNonRevenue()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDataInvoiceNonRevenue(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoiceNonRevenueID { get; set; }
		public string InvNo { get; set; }
		public string CompanyID { get; set; }
		public string Company { get; set; }
		public string OperatorID { get; set; }
		public string Customer { get; set; }
		public DateTime? InvoiceDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string Status { get; set; }
		public string TermInvoice { get; set; }
		public decimal? Amount { get; set; }
		public decimal? InvoiceTotal { get; set; }
		public decimal? Discount { get; set; }
		public decimal? DPP { get; set; }
		public decimal? TotalPPN { get; set; }
		public decimal? TotalPPH { get; set; }
		public decimal? Penalty { get; set; }
		public string Currency { get; set; }
        public int mstInvoiceStatusId { get; set; }
        public string InvSubject { get; set; }
        public int? mstCategoryInvoiceID { get; set; }

    }
}