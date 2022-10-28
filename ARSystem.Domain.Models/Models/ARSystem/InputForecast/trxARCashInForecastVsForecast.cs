
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ARSystemFrontEnd.Domain.Models
{
    public class trxARCashInForecastVsForecast : BaseClass
	{
		public trxARCashInForecastVsForecast()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxARCashInForecastVsForecast(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string OperatorID { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
		public int? Quarter { get; set; }
		public int? Week { get; set; }
		public decimal? FCLastMonthIDR { get; set; }
		public decimal? FCLastMonthUSD { get; set; }
		public decimal? FCCurrentMonthIDR { get; set; }
		public decimal? FCCurrentMonthUSD { get; set; }
		public string LastApprovalAction { get; set; }
		public string LastApprovalRemarks { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}