
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class MstInitialDepartment : BaseClass
	{
		public MstInitialDepartment()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public MstInitialDepartment(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public string DepartmentCode { get; set; }
        public string DeptCode { get; set; }


    }
}