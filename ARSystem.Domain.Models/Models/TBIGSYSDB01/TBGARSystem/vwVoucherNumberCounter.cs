
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwVoucherNumberCounter : BaseClass
	{
		public vwVoucherNumberCounter()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwVoucherNumberCounter(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int Counter { get; set; }
		public string OperatorId { get; set; }
		public string CompanyId { get; set; }
		public int mstPaymentCodeId { get; set; }
		public string Code { get; set; }
	}
}