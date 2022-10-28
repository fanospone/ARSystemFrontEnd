
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwEmployee : BaseClass
	{
		public vwEmployee()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwEmployee(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string UserID { get; set; }
		public string EmployeeID { get; set; }
		public string Name { get; set; }
		public string NickName { get; set; }
		public string PlaceOfBirth { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public string PhoneNumber { get; set; }
		public string MobilePhoneNumber { get; set; }
		public string Email { get; set; }
		public string IDNumber { get; set; }
		public string IDAddress { get; set; }
		public string IDCity { get; set; }
		public string IDZipCode { get; set; }
		public DateTime? JoinDate { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public string NPWPID { get; set; }
		public DateTime? NPWPDate { get; set; }
		public string JamsostekID { get; set; }
		public DateTime? JamsostekDate { get; set; }
		public string BankCode { get; set; }
		public string AccountBankNumber { get; set; }
		public string AccountBankName { get; set; }
		public int? VendorID { get; set; }
		public int? EmployeeStructureID { get; set; }
		public string DepartmentName { get; set; }
		public string DepartmentCode { get; set; }
		public string DivisionName { get; set; }
		public string DivisionCode { get; set; }
		public string DirectorateName { get; set; }
		public string DirectorateCode { get; set; }
		public string PositionName { get; set; }
		public string PositionCode { get; set; }
		public bool? EmployeeStructureActive { get; set; }
	}
}