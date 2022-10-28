
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstDepartment : BaseClass
	{
		public mstDepartment()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstDepartment(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string DepartmentName { get; set; }
		public string DepartmentCode { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string DirectorateCode { get; set; }
		public string DivisionCode { get; set; }
		public string DepartmentInitial { get; set; }
	}
}