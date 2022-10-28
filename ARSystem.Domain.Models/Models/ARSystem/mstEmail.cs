
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstEmail : BaseClass
	{
		public mstEmail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstEmail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstEmailID { get; set; }
		public string EmailName { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Query { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}