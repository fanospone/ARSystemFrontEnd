
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxARRevSysAdjustment : BaseClass
	{
		public trxARRevSysAdjustment()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxARRevSysAdjustment(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int Id { get; set; }
		public int? RemarksAdjustmentID { get; set; }
		public string SoNumber { get; set; }
		public decimal? Amount { get; set; }
		public string MonthPeriod { get; set; }
		public int? MonthPeriodNumber { get; set; }
		public int? YearPeriod { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
	}
}