
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxBapsStaging : BaseClass
	{
		public TrxBapsStaging()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxBapsStaging(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxBapsDataId { get; set; }
		public string SONumber { get; set; }
		public string BapsType { get; set; }
		public string BapsPeriod { get; set; }
		public string Currency { get; set; }
		public string StipSiro { get; set; }
		public string PowerTypeCode { get; set; }
		public int? InvoiceStatusId { get; set; }
	}
}