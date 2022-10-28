
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class MstRASowProduct : BaseClass
	{
		public MstRASowProduct()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public MstRASowProduct(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int SowProductId { get; set; }
		public string SowName { get; set; }
		public int? SectionProductId { get; set; }
	}
}