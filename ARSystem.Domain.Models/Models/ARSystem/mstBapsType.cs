
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstBapsType : BaseClass
	{
		public mstBapsType()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstBapsType(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstBapsTypeId { get; set; }
		public string BapsType { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}