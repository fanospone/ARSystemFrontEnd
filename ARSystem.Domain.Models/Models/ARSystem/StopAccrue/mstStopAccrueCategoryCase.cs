
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstStopAccrueCategoryCase : BaseClass
	{
		public mstStopAccrueCategoryCase()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstStopAccrueCategoryCase(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string CategoryCase { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}