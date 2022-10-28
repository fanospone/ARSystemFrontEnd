
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstAccrueRootCause : BaseClass
	{
		public mstAccrueRootCause()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstAccrueRootCause(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string RootCause { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}