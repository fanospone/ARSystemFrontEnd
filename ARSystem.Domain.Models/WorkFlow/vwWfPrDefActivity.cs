
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwWfPrDefActivity : BaseClass
	{
		public vwWfPrDefActivity()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwWfPrDefActivity(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ActivityID { get; set; }
		public string Activity { get; set; }
		public int ProcessID { get; set; }
		public int? JobID { get; set; }
		public string Code { get; set; }
	}
}