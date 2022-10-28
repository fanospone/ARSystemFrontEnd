
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwWfHeaderActivityLogs : BaseClass
	{
		public vwWfHeaderActivityLogs()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwWfHeaderActivityLogs(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int LogID { get; set; }
		public int? AppHeaderID { get; set; }
		public string LogText { get; set; }
		public DateTime? LogTime { get; set; }
		public string ByNIP { get; set; }
		public string FollowUp { get; set; }
		public string Activity { get; set; }
		public int ActivityID { get; set; }
		public string Label { get; set; }
		public string name { get; set; }
        public string EventCode { get; set; }
        public string PositionName { get; set; }
        public string ActivityLabel { get; set; }
    }
}