
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwDataCreateInvoiceNonRevenue : BaseClass
	{
		public vwDataCreateInvoiceNonRevenue()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDataCreateInvoiceNonRevenue(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}

        public Int64 RowNumber { get; set; }
		public string SoNumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string SiteIDCustomer { get; set; }
		public string SiteNameCustomer { get; set; }
		public string CompanyID { get; set; }
		public string OperatorID { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public decimal Amount { get; set; }
    }
}