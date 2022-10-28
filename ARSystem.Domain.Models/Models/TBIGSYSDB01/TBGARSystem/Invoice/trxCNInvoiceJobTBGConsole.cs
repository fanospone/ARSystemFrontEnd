
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCNInvoiceJobTBGConsole : BaseClass
	{
		public trxCNInvoiceJobTBGConsole()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCNInvoiceJobTBGConsole(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
        public int? Status { get; set; }
		public string Response { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdateBy { get; set; }
	}
}