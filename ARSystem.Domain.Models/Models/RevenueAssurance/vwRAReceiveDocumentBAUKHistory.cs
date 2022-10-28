
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwRAReceiveDocumentBAUKHistory : BaseClass
	{
		public vwRAReceiveDocumentBAUKHistory()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRAReceiveDocumentBAUKHistory(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string DeptSubmitBAUK { get; set; }
		public DateTime? BaukSubmitBySystem { get; set; }
		public string SONumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public int? ProductID { get; set; }
		public string Product { get; set; }
		public string CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string CompanyID { get; set; }
		public string Company { get; set; }
		public int? STIPID { get; set; }
		public string STIPCode { get; set; }
		public string StatusDoc { get; set; }
	}
}