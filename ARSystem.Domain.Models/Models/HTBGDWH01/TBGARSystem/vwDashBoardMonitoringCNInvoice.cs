
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.HTBGDWH01.TBGARSystem
{
	public class vwDashBoardMonitoringCNInvoice : BaseClass
	{
		public vwDashBoardMonitoringCNInvoice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDashBoardMonitoringCNInvoice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string CompanyID { get; set; }
		public string CompanyName { get; set; }
		public int? OD_13 { get; set; }
		public int? OD_46 { get; set; }
		public int? OD_79 { get; set; }
		public int? OD_9s { get; set; }
		public int? GrandTotal { get; set; }
	}
}