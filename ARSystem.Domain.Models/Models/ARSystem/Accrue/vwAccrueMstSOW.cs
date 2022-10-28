
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwAccrueMstSOW : BaseClass
	{
		public vwAccrueMstSOW()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwAccrueMstSOW(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SOW { get; set; }
	}
}