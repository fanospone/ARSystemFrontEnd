
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstARRevSysParameter : BaseClass
	{
		public mstARRevSysParameter()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstARRevSysParameter(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string ParamName { get; set; }
		public string ParamValue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}