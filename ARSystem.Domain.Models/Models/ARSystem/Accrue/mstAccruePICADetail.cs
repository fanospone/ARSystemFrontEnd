
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstAccruePICADetail : BaseClass
	{
		public mstAccruePICADetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstAccruePICADetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string PICADetail { get; set; }
		public int AccruePICAID { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}