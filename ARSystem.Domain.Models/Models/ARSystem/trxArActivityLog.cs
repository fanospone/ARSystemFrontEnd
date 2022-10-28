
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxArActivityLog : BaseClass
	{
		public trxArActivityLog()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxArActivityLog(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxArActivityLogId { get; set; }
		public int TrxArHeaderId { get; set; }
		public string Action { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}