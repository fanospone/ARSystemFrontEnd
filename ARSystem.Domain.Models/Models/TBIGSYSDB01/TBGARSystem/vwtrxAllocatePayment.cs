
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwtrxAllocatePayment : BaseClass
	{
		public vwtrxAllocatePayment()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwtrxAllocatePayment(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long trxAllocatePaymentBankInID { get; set; }
		public DateTime? PaidDate { get; set; }
        public string Type { get; set; }
        public string CompanyID { get; set; }
		public string OperatorID { get; set; }
		public decimal? Amount { get; set; }
		public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? Unsettle { get; set; }
		public string Status { get; set; }
        public decimal? AmountBankOut { get; set; }
    }
}