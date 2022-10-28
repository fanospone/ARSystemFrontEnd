
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class MstRASectionProduct : BaseClass
	{
		public MstRASectionProduct()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public MstRASectionProduct(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int SectionProductId { get; set; }
		public string SectionName { get; set; }
	}
}