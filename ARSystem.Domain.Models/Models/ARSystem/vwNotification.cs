
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwNotification : BaseClass
	{
		public vwNotification()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwNotification(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int? TaskCount { get; set; }
		public string TaskUrl { get; set; }
		public string TaskName { get; set; }
		public int Sort { get; set; }
	}
}