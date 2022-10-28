
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstStopAccrueType : BaseClass
	{
		public mstStopAccrueType()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstStopAccrueType(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string AccrueType { get; set; }
		public bool? IsActive { get; set; }
	}
}