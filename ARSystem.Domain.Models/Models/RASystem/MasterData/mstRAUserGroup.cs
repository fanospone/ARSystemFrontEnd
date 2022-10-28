
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstRAUserGroup : BaseClass
	{
		public mstRAUserGroup()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstRAUserGroup(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string UserID { get; set; }
		public string UserGroup { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}