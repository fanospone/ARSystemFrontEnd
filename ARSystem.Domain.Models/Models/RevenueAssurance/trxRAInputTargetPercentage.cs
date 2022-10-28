
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRAInputTargetPercentage : BaseClass
	{
		public trxRAInputTargetPercentage()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRAInputTargetPercentage(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public string Department { get; set; }
		public decimal Optimist { get; set; }
		public decimal MostLikely { get; set; }
		public decimal Pessimist { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}