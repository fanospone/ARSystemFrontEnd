
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwDwhAPTenantRevenue : BaseClass
	{
		public vwDwhAPTenantRevenue()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDwhAPTenantRevenue(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public long RowIndex { get; set; }
		public string SONumber { get; set; }
		public string Product { get; set; }
		public string CustomerID { get; set; }
		public DateTime? InvoiceDate { get; set; }
		public string TypePayment { get; set; }
		public string Period { get; set; }
		public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }

    }
}