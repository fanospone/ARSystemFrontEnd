
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwReportInvoiceMatchingAR : BaseClass
	{
		public vwReportInvoiceMatchingAR()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReportInvoiceMatchingAR(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public string InvoiceNumber { get; set; }
		public DateTime? InvPaidDate { get; set; }
		public string CompanyCodeInvoice { get; set; }
		public string Customer { get; set; }
		public decimal? PaidAmount { get; set; }
		public string DocumentPayment { get; set; }
		public string Keterangan { get; set; }
		public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
    }
}