
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Domain.Models
{
    public class uspARCashInForecastVsForecast : BaseClass
    {
        public uspARCashInForecastVsForecast()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public uspARCashInForecastVsForecast(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public string OperatorID { get; set; }
        public int? Month { get; set; }
        public int? MonthWithinQuarter { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }
        public decimal? TotalForecastM1Idr { get; set; }
        public decimal? TotalForecastM2Idr { get; set; }
        public decimal? TotalForecastM3Idr { get; set; }
        public decimal? TotalForecastM1Usd { get; set; }
        public decimal? TotalForecastM2Usd { get; set; }
        public decimal? TotalForecastM3Usd { get; set; }
        public decimal? TotalLastPeriodIdr { get; set; }
        public decimal? TotalLastPeriodUsd { get; set; }
        public decimal? TotalActualM1Idr { get; set; }
        public decimal? TotalActualM2Idr { get; set; }
        public decimal? TotalActualM3Idr { get; set; }
        public decimal? TotalActualM1Usd { get; set; }
        public decimal? TotalActualM2Usd { get; set; }
        public decimal? TotalActualM3Usd { get; set; }
        public decimal? TotalCurrentPeriodIdr { get; set; }
        public decimal? TotalCurrentPeriodUsd { get; set; }
        public decimal? VarianceIdr { get; set; }
        public decimal? VarianceUsd { get; set; }
        public string Remarks { get; set; }
        public string PiCa { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string LastApprovalAction { get; set; }
        public string LastApprovalRemarks { get; set; }
    }
}
