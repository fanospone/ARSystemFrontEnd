
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class logCounterInvoiceHeader : BaseClass
	{
		public logCounterInvoiceHeader()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logCounterInvoiceHeader(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long LogId { get; set; }
		public long Counter { get; set; }
		public string CompanyID { get; set; }
		public int? InvoiceYear { get; set; }
		public int? MstInvoiceCategoryID { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}