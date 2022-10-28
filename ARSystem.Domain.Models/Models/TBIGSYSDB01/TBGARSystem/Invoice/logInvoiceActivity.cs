
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class logInvoiceActivity : BaseClass
	{
		public logInvoiceActivity()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logInvoiceActivity(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int logInvoiceActivityId { get; set; }
		public string InvNo { get; set; }
		public int mstInvoiceStatusId { get; set; }
		public DateTime LogDate { get; set; }
	}
}