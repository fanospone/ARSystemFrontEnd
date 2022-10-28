
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class idxFINInvoiceReplacement : BaseClass
	{
		public idxFINInvoiceReplacement()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public idxFINInvoiceReplacement(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int InvoiceReplacementID { get; set; }
		public string CNInvoiceNo { get; set; }
		public string InvoiceNo { get; set; }
		public string FakturNo { get; set; }
		public string ReplacedBy { get; set; }
		public DateTime? ReplacedDate { get; set; }
		public bool? isReplaceable { get; set; }
	}
}