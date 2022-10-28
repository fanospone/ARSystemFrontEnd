
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxARReOpenPaymentDateDetail : BaseClass
	{
		public TrxARReOpenPaymentDateDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxARReOpenPaymentDateDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}

        public Int64 RowIndex { get; set; }
        public int ID { get; set; }
		public int TrxARReOpenPaymentDateID { get; set; }
		public int TrxInvoiceHeaderID { get; set; }
        public string InvNo { get; set; }
        public DateTime? PaymentDate { get; set; }
		public DateTime? CreateDate { get; set; }
		public string CreatedBy { get; set; }
	}
}