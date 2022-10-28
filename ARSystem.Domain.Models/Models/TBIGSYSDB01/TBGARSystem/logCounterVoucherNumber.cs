
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class logCounterVoucherNumber : BaseClass
	{
		public logCounterVoucherNumber()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public logCounterVoucherNumber(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int logCounterVoucherNumberId { get; set; }
		public int mstPaymentCodeId { get; set; }
		public int Counter { get; set; }
		public string OperatorId { get; set; }
		public string CompanyId { get; set; }
		public string VoucherNumberYear { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}