
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstPaymentCode : BaseClass
	{
		public mstPaymentCode()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstPaymentCode(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstPaymentCodeId { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string COA { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}