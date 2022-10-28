
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxRAAmountTargetLog : BaseClass
	{
		public TrxRAAmountTargetLog()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxRAAmountTargetLog(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int TrxRAAmountTargetID { get; set; }
		public int? Month { get; set; }
		public decimal? AmountTarget { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}