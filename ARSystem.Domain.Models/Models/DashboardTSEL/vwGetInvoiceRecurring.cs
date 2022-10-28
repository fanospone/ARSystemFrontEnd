
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwGetInvoiceRecurring : BaseClass
	{
		public vwGetInvoiceRecurring()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwGetInvoiceRecurring(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxInvoiceTowerDetailId { get; set; }
		public int trxInvoiceHeaderID { get; set; }
		public string SONumber { get; set; }
		public string BapsPeriod { get; set; }
		public DateTime? StartDatePeriod { get; set; }
		public DateTime? EndDatePeriod { get; set; }
		public string PoNumber { get; set; }
		public string Type { get; set; }
		public string BapsType { get; set; }
		public int mstBapsTypeId { get; set; }
		public string PowerType { get; set; }
		public string PowerTypeCode { get; set; }
		public decimal AmountInvoicePeriod { get; set; }
		public int? YearBill { get; set; }
		public int? MonthBill { get; set; }
		public string MonthBillName { get; set; }
		public string CustomerID { get; set; }
        public int Qty { get; set; }
        public int Targets { get; set; }

    }
}