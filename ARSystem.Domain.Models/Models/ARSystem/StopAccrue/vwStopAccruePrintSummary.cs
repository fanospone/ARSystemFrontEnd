
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwStopAccruePrintSummary : BaseClass
	{
		public vwStopAccruePrintSummary()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwStopAccruePrintSummary(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int LogID { get; set; }
		public int? AppHeaderID { get; set; }
		public DateTime? ApprovalDate { get; set; }
		public string ActivityLabel { get; set; }
		public string FollowUp { get; set; }
		public string ApprovalName { get; set; }
		public string PositionName { get; set; }
	}
}