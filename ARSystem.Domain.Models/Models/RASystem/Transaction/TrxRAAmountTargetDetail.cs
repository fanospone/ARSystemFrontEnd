
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxRAAmountTargetDetail : BaseClass
	{
		public TrxRAAmountTargetDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxRAAmountTargetDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int? TrxRAAmountTargetID { get; set; }
		public int? Month { get; set; }
		public decimal? AmountTarget { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}