
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxARRMonitoringCashInRemarkApprovalLog : BaseClass
	{
		public TrxARRMonitoringCashInRemarkApprovalLog()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxARRMonitoringCashInRemarkApprovalLog(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string Action { get; set; }
		public string Remarks { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? TrxARRMonitoringCashInRemarkHeaderID { get; set; }
	}
}