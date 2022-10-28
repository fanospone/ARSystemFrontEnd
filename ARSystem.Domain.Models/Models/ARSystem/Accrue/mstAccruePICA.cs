
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstAccruePICA : BaseClass
	{
		public mstAccruePICA()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstAccruePICA(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string PICA { get; set; }
		public int AccrueRootCauseID { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}