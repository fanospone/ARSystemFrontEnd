
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class TrxARRMonitoringCashInRemarkDetailQuarterly : BaseClass
	{
		public TrxARRMonitoringCashInRemarkDetailQuarterly()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public TrxARRMonitoringCashInRemarkDetailQuarterly(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string OperatorID { get; set; }
		public string Remarks { get; set; }
		public decimal? ForecastLastQA { get; set; }
		public decimal? ForecastLastQB { get; set; }
		public decimal? ForecastLastQC { get; set; }
		public decimal? ForecastLastTotal { get; set; }
		public decimal? ForecastCurrentQA { get; set; }
		public decimal? ForecastCurrentQB { get; set; }
		public decimal? ForecastCurrentQC { get; set; }
		public decimal? ForecastCurrentTotal { get; set; }
		public decimal? Variance { get; set; }
		public int Periode { get; set; }
		public int Quarter { get; set; }
		public string Status { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? TrxARRMonitoringCashInRemarkHeaderID { get; set; }
	}
}