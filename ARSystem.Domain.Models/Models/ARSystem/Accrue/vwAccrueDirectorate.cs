
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwAccrueDirectorate : BaseClass
	{
		public vwAccrueDirectorate()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwAccrueDirectorate(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string DirectorateCode { get; set; }
		public string DirectorateName { get; set; }
	}
}