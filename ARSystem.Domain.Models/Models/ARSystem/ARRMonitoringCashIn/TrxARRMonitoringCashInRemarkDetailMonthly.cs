
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxARRMonitoringCashInRemarkDetailMonthly : BaseClass
	{
		public TrxARRMonitoringCashInRemarkDetailMonthly()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxARRMonitoringCashInRemarkDetailMonthly(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string OperatorID { get; set; }
		public string Remarks { get; set; }
		public decimal? TotalForecast { get; set; }
		public decimal? TotalActual { get; set; }
		public decimal? TotalVariance { get; set; }
		public decimal? PercentageVariance { get; set; }
		public int Periode { get; set; }
		public int Month { get; set; }
		public string Status { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? TrxARRMonitoringCashInRemarkHeaderID { get; set; }
	}
}