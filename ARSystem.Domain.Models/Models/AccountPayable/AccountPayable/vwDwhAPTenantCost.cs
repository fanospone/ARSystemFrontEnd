
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwDwhAPTenantCost : BaseClass
	{
		public vwDwhAPTenantCost()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDwhAPTenantCost(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public long RowIndex { get; set; }
        public string DocumentNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public string Description { get; set; }
		public string Termin { get; set; }
		public decimal? Amount { get; set; }
		public string SONumber { get; set; }
		public string VOUCHER { get; set; }
		public string SourceData { get; set; }
		public DateTime? TRANSDATE { get; set; }
	}
}