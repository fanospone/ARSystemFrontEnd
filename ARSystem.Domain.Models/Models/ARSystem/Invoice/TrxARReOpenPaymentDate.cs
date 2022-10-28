
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxARReOpenPaymentDate : BaseClass
	{
		public TrxARReOpenPaymentDate()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxARReOpenPaymentDate(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}

        public Int64 RowIndex { get; set; }
        public int ID { get; set; }
		public int ApprStatusID { get; set; }
        public string RequestNumber { get; set; }
        public string ApprStatus { get; set; }
		public DateTime? PaymentDateRevision { get; set; }
        public DateTime? PaymentDateRevision2 { get; set; }
        public string Remarks { get; set; }
		public DateTime? CreateDate { get; set; }
        public DateTime? CreateDate2 { get; set; }
        public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}