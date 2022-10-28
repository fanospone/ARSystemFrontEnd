
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstCompany : BaseClass
	{
		public mstCompany()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstCompany(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string CompanyID { get; set; }
		public string CompanyIDAX { get; set; }
		public string Company { get; set; }
		public bool? IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}