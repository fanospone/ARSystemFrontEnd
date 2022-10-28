
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstCustomer : BaseClass
	{
		public mstCustomer()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstCustomer(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string CustomerID { get; set; }
		public string CustomerCode { get; set; }
		public string CustomerName { get; set; }
		public string CompanyName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string NPWP { get; set; }
		public bool? IsTelco { get; set; }
		public bool? IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}