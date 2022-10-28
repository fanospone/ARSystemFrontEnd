
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxAllocatePaymentBankIn : BaseClass
	{
		public trxAllocatePaymentBankIn()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxAllocatePaymentBankIn(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long trxAllocatePaymentBankInID { get; set; }
		public DateTime? PaidDate { get; set; }
        public int TypeID { get; set; }
        public string CompanyID { get; set; }
		public string OperatorID { get; set; }
		public decimal? Amount { get; set; }
		public string Description { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}