
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwCustomerReceiveBAUK : BaseClass
	{
		public vwCustomerReceiveBAUK()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwCustomerReceiveBAUK(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string OperatorId { get; set; }
		public string Operator { get; set; }
	}
}