
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwTrxAmountTarget : BaseClass
	{
		public vwTrxAmountTarget()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwTrxAmountTarget(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int TargetAmountID { get; set; }
		public int ApprovalStatusID { get; set; }
		public string StatusName { get; set; }
		public string StatusCode { get; set; }
		public string CustomerID { get; set; }
		public int Year { get; set; }
		public int? Month { get; set; }
		public decimal? AmountTarget { get; set; }
	}
}