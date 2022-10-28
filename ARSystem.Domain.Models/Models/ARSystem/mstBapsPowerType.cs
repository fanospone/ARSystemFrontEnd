
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstBapsPowerType : BaseClass
	{
		public mstBapsPowerType()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstBapsPowerType(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string KodeType { get; set; }
		public string BapsType { get; set; }
		public string PowerType { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}