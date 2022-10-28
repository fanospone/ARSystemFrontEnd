
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxArHeader : BaseClass
	{
		public trxArHeader()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxArHeader(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxArHeaderId { get; set; }
		public string BatchNo { get; set; }
		public long SONumberAmount { get; set; }
		public decimal TotalAmount { get; set; }
		public string Activity { get; set; }
		public string Status { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}