
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxARRMonitoringCashInRemarkHeader : BaseClass
	{
		public TrxARRMonitoringCashInRemarkHeader()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxARRMonitoringCashInRemarkHeader(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string RemarkType { get; set; }
		public bool? IsApproved { get; set; }
		public string OperatorID { get; set; }
		public int? Periode { get; set; }
		public int? Month { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}