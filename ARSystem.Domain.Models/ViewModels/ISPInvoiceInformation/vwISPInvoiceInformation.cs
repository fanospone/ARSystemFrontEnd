
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwISPInvoiceInformation : BaseClass
	{
		public vwISPInvoiceInformation()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwISPInvoiceInformation(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SONumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
        public string CompanyID { get; set; }
        public string Company { get; set; }
        public string OperatorID { get; set; }
        public string CustomerName { get; set; }
        public int? STIPID { get; set; }
        public string STIPCode { get; set; }
        public string STIPDescription { get; set; }
        public string Status { get; set; }
        public string RFIDate { get; set; }
    }
}