
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstApprovalStatus : BaseClass
	{
		public mstApprovalStatus()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstApprovalStatus(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string StatusName { get; set; }
		public string StatusCode { get; set; }
		public int? StatusType { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}