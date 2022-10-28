
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstEmployee : BaseClass
	{
		public mstEmployee()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstEmployee(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string EmployeeId { get; set; }
		public string EmployeeIdTBGSYS { get; set; }
		public string BadgeId { get; set; }
		public string Name { get; set; }
		public string NickName { get; set; }
		public decimal? GenderCode { get; set; }
		public string PlaceOfBirth { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public decimal? ReligionCode { get; set; }
		public decimal? MaritalStatusCode { get; set; }
		public decimal? BloodTypeCode { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public string Phone { get; set; }
		public string HpNo { get; set; }
		public string Email { get; set; }
		public string IdNo { get; set; }
		public string IdAddress { get; set; }
		public string IdCity { get; set; }
		public string IdZipCode { get; set; }
		public string CertificatedNo { get; set; }
		public DateTime? JoinDate { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public string ResignStatus { get; set; }
		public DateTime? ResignDate { get; set; }
		public decimal? JobStatusCode { get; set; }
		public decimal? EmployeeStatusCode { get; set; }
		public DateTime? ExpiredStatus { get; set; }
		public decimal? ContractExtend { get; set; }
		public string SchedulingShift { get; set; }
		public string Shift { get; set; }
		public decimal? TaxStatusCode { get; set; }
		public string TaxCalculation { get; set; }
		public string NpwpNo { get; set; }
		public DateTime? NpwpDate { get; set; }
		public string JamsostekNo { get; set; }
		public DateTime? JamsostekDate { get; set; }
		public string DplkNo { get; set; }
		public DateTime? DplkDate { get; set; }
		public decimal? EmpContribution { get; set; }
		public decimal? CompanyCode { get; set; }
		public string BranchCode { get; set; }
		public string SectionCode { get; set; }
		public string JobTitleCode { get; set; }
		public decimal? GradeCode { get; set; }
		public string SubGrade { get; set; }
		public string ReportCode { get; set; }
		public string PaymentType { get; set; }
		public string BankCode { get; set; }
		public string AccountNo { get; set; }
		public string AccountName { get; set; }
		public string PositionCode { get; set; }
		public string PositionName { get; set; }
		public string DepartmentCode { get; set; }
		public string DepartmentName { get; set; }
		public string DivisionCode { get; set; }
		public string DivisionName { get; set; }
		public string DirectorateCode { get; set; }
		public string DirectorateName { get; set; }
		public string EmployeeIdHcis { get; set; }
		public string LevelPosition { get; set; }
		public string LevelPositionId { get; set; }
		public int? IdVendOs { get; set; }
		public string JobTitleId { get; set; }
		public string JobTitleName { get; set; }
		public string WorkLocation { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}