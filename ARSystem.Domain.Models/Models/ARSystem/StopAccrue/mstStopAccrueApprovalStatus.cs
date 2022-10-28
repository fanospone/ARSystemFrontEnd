
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstStopAccrueApprovalStatus : BaseClass
	{
		public mstStopAccrueApprovalStatus()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstStopAccrueApprovalStatus(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string StatusApproval { get; set; }
		public string RoleLabel { get; set; }
		public bool? IsShowUp { get; set; }
        public string ActivityLabel { get; set; }
        public int? Sort { get; set; }
	}
}