
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwDataARUser : BaseClass
	{
		public vwDataARUser()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDataARUser(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string Position { get; set; }
	}
}