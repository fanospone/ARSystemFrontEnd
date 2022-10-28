
using System;


namespace ARSystem.Domain.Models
{
	public class vwMstDepartment : BaseClass
	{
		public vwMstDepartment()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwMstDepartment(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}

        public Int64 RowIndex { get; set; }
        public string DepartmentCode { get; set; }
		public string DepartmentName { get; set; }
		public string DepartmentInitial { get; set; }
		public string DivisionCode { get; set; }
		public string DivisionName { get; set; }
		public string DivisionInitial { get; set; }
		public string DirectorateCode { get; set; }
		public string DirectorateName { get; set; }
		public string DirectorateInitial { get; set; }
	}
}