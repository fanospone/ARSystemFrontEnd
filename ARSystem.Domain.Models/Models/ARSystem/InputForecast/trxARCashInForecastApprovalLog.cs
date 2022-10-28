using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Domain.Models
{
	public class trxARCashInForecastApprovalLog : BaseClass
	{
		public trxARCashInForecastApprovalLog()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxARCashInForecastApprovalLog(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string Action { get; set; }
		public string Remarks { get; set; }
        public string ActionBy { get; set; }
		public DateTime? ActionDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int ForecastYear { get; set; }
		public int? ForecastMonth { get; set; }
		public int ForecastQuarter { get; set; }
		public int? ForecastWeek { get; set; }
		public string ForecastType { get; set; }
		public string ApprovalType { get; set; }
        public int? ApprovalSequence { get; set; }
        public decimal? ProcessOID { get; set; }

    }
}