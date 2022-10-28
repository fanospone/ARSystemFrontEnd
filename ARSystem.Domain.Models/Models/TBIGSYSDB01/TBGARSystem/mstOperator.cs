
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstOperator : BaseClass
	{
		public mstOperator()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstOperator(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string OprCd { get; set; }
		public string OperatorCode { get; set; }
		public string OperatorId { get; set; }
		public string Operator { get; set; }
		public string PopularName { get; set; }
		public string OperatorDesc { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string Telp { get; set; }
		public string Fax { get; set; }
		public string Hp { get; set; }
		public string ContactPerson { get; set; }
		public string Section { get; set; }
		public string Npwp { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string iCustomerID_EIS { get; set; }
		public string LastId { get; set; }
	}
}