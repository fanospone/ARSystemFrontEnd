
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Domain.Models
{
    public class uspARCashInForecastVsActual : BaseClass
    {
        public uspARCashInForecastVsActual()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public uspARCashInForecastVsActual(int errorType, string errorMessage)
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
        public int Week { get; set; }
        public decimal? FCMarketingM1 { get; set; }
        public decimal? FCMarketingM2 { get; set; }
        public decimal? FCMarketingM3 { get; set; }
        public decimal? FCRevenueAssuranceM1 { get; set; }
        public decimal? FCRevenueAssuranceM2 { get; set; }
        public decimal? FCRevenueAssuranceM3 { get; set; }
        public decimal? FCFinanceM1 { get; set; }
        public decimal? FCFinanceM2 { get; set; }
        public decimal? FCFinanceM3 { get; set; }
        public decimal? ActualM1 { get; set; }
        public decimal? ActualM2 { get; set; }
        public decimal? ActualM3 { get; set; }
        public decimal? VarianceM1 { get; set; }
        public decimal? VarianceM2 { get; set; }
        public decimal? VarianceM3 { get; set; }
        public string PiCaM1 { get; set; }
        public string PiCaM2 { get; set; }
        public string PiCaM3 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string LastApprovalAction { get; set; }
        public string LastApprovalRemarks { get; set; }
    }
}
