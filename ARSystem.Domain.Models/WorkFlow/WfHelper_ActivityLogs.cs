
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class WfHelper_ActivityLogs : BaseClass
	{
		public WfHelper_ActivityLogs()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public WfHelper_ActivityLogs(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int LogID { get; set; }
		public int? AppHeaderID { get; set; }
		public string LogText { get; set; }
		public string EventCode { get; set; }
		public DateTime? LogTime { get; set; }
		public string ByNIP { get; set; }
		public string Label { get; set; }
		public string FollowUp { get; set; }
		public string ByJobTitle { get; set; }
		public string ByLocation { get; set; }
	}
}