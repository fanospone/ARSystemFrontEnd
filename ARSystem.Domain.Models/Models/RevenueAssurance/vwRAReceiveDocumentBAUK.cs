
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwRAReceiveDocumentBAUK : BaseClass
	{
		public vwRAReceiveDocumentBAUK()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRAReceiveDocumentBAUK(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
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
		public string PICReceive { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public string Remarks { get; set; }
        public DateTime? StartSubmit { get; set; }
        public DateTime? EndSubmit { get; set; }

    }
}