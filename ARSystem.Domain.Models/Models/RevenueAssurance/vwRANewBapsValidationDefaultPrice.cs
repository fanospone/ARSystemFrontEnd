
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwRANewBapsValidationDefaultPrice : BaseClass
	{
		public vwRANewBapsValidationDefaultPrice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRANewBapsValidationDefaultPrice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public decimal? First5Year { get; set; }
		public decimal? OMPrice { get; set; }
		public string SONumber { get; set; }
	}
}