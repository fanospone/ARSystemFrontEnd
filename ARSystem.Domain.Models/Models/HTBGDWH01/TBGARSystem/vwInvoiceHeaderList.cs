
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.HTBGDWH01.TBGARSystem
{
	public class vwInvoiceHeaderList : BaseClass
	{
		public vwInvoiceHeaderList()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwInvoiceHeaderList(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string InvNo { get; set; }
		public string TaxNo { get; set; }
		public string OperatorID { get; set; }
	}
}