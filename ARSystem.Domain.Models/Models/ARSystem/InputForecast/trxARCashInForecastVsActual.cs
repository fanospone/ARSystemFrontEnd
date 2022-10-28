
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Domain.Models
{
	public class trxARCashInForecastVsActual : BaseClass
	{
		public trxARCashInForecastVsActual()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxARCashInForecastVsActual(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string OperatorID { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
		public int? Quarter { get; set; }
		public int Week { get; set; }
		public decimal? FCMarketing { get; set; }
		public decimal? FCRevenueAssurance { get; set; }
		public decimal? FCFinance { get; set; }
		public decimal? Actual { get; set; }
		public string PiCa { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string LastApprovalAction { get; set; }
		public string LastApprovalRemarks { get; set; }
	}
}