
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwChangePPHFinal : BaseClass
	{
		public vwChangePPHFinal()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwChangePPHFinal(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int UniqueID { get; set; }
		public string InvoiceNumber { get; set; }
		public string SONumber { get; set; }
		public string BAPSNumber { get; set; }
		public string BAPSType { get; set; }
		public string BAPSPeriod { get; set; }
		public string PONumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteIDOpr { get; set; }
		public string SiteNameOpr { get; set; }
		public string Type { get; set; }
		public string OperatorID { get; set; }
		public int? STIPSiroID { get; set; }
		public string CompanyID { get; set; }
		public string Company { get; set; }
		public DateTime? StartDateInvoice { get; set; }
		public DateTime? EndDateInvoice { get; set; }
		public bool? IsPPHFinal { get; set; }
	}
}