
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwARUserRole : BaseClass
	{
		public vwARUserRole()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwARUserRole(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string UserID { get; set; }
		public string UserName { get; set; }
		public string Position { get; set; }
		public string HCISPosition { get; set; }
	}
}