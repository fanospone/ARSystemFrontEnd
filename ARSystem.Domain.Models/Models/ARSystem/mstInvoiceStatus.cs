
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstInvoiceStatus : BaseClass
	{
		public mstInvoiceStatus()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstInvoiceStatus(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstInvoiceStatusId { get; set; }
		public string Description { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}