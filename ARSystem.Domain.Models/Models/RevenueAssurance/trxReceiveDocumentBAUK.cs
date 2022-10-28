
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxReceiveDocumentBAUK : BaseClass
	{
		public trxReceiveDocumentBAUK()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxReceiveDocumentBAUK(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
		public int trxReceiveDocID { get; set; }
		public DateTime? BaukSubmitBySystem { get; set; }
		public string SONumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string CustomerName { get; set; }
		public string Remarks { get; set; }
		public string StatusDocument { get; set; }
		public string PICReceive { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}