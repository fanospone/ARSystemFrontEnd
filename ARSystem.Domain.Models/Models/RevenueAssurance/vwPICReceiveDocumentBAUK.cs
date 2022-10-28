
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwPICReceiveDocumentBAUK : BaseClass
	{
		public vwPICReceiveDocumentBAUK()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwPICReceiveDocumentBAUK(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string UserID { get; set; }
		public string FullName { get; set; }
		public string HCISPosition { get; set; }
	}
}