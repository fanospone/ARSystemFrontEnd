
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.HTBGDWH01.TBGARSystem
{
	public class vwDashBoardMonitoringCNInvoiceDetail : BaseClass
	{
		public vwDashBoardMonitoringCNInvoiceDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDashBoardMonitoringCNInvoiceDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string InvNumber { get; set; }
		public string NoFaktur { get; set; }
		public string CompanyInvoice { get; set; }
		public string CustomerID { get; set; }
		public string Subject { get; set; }
		public decimal? AmountInvoice { get; set; }
		public string CustomerName { get; set; }
		public string CompanyID { get; set; }
		public string CompanyName { get; set; }
		public int OD_13 { get; set; }
		public int OD_46 { get; set; }
		public int OD_79 { get; set; }
		public int OD_9s { get; set; }
	}
}