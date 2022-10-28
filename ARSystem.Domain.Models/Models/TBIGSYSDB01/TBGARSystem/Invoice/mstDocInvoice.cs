
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstDocInvoice : BaseClass
	{
		public mstDocInvoice()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstDocInvoice(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int DocInvoiceID { get; set; }
		public string OperatorCode { get; set; }
		public string OperatorID { get; set; }
		public string DocName { get; set; }
		public string DocType { get; set; }
		public string PICDoc { get; set; }
		public string IsActiveARData { get; set; }
		public string IsActiveARCollection { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}