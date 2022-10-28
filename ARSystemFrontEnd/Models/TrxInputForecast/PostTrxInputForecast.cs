using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models.TrxInputForecast
{
    public class PostTrxInputForecast : DatatableAjaxModel
    {
        public int ID { get; set; }
        public string OperatorID { get; set; }
        public int? Month { get; set; }
        public int? MonthWithinQuarter { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }
        public int Week { get; set; }
        public decimal? FCMarketing { get; set; }
        public decimal? FCMarketingM1 { get; set; }
        public decimal? FCMarketingM2 { get; set; }
        public decimal? FCMarketingM3 { get; set; }
        public decimal? FCRevenueAssurance { get; set; }
        public decimal? FCRevenueAssuranceM1 { get; set; }
        public decimal? FCRevenueAssuranceM2 { get; set; }
        public decimal? FCRevenueAssuranceM3 { get; set; }
        public decimal? FCFinance { get; set; }
        public decimal? FCFinanceM1 { get; set; }
        public decimal? FCFinanceM2 { get; set; }
        public decimal? FCFinanceM3 { get; set; }
        public decimal? Actual { get; set; }
        public decimal? ActualM1 { get; set; }
        public decimal? ActualM2 { get; set; }
        public decimal? ActualM3 { get; set; }
        public decimal? Variance { get; set; }
        public decimal? VarianceIdr { get; set; }
        public decimal? VarianceUsd { get; set; }
        public decimal? VarianceM1 { get; set; }
        public decimal? VarianceM2 { get; set; }
        public decimal? VarianceM3 { get; set; }
        public decimal? VarianceM1Idr { get; set; }
        public decimal? VarianceM2Idr { get; set; }
        public decimal? VarianceM3Idr { get; set; }
        public decimal? VarianceM1Usd { get; set; }
        public decimal? VarianceM2Usd { get; set; }
        public decimal? VarianceM3Usd { get; set; }
        public string PiCa { get; set; }
        public string PiCaM1 { get; set; }
        public string PiCaM2 { get; set; }
        public string PiCaM3 { get; set; }
        public string Remarks { get; set; }

        public decimal? TotalForecastM1Idr { get; set; }
        public decimal? TotalForecastM1Usd { get; set; }
        public decimal? TotalForecastM2Idr { get; set; }
        public decimal? TotalForecastM2Usd { get; set; }
        public decimal? TotalForecastM3Idr { get; set; }
        public decimal? TotalForecastM3Usd { get; set; }

        public decimal? TotalActualM1Idr { get; set; }
        public decimal? TotalActualM1Usd { get; set; }
        public decimal? TotalActualM2Idr { get; set; }
        public decimal? TotalActualM2Usd { get; set; }
        public decimal? TotalActualM3Idr { get; set; }
        public decimal? TotalActualM3Usd { get; set; }

        public decimal? TotalCurrentPeriodIdr { get; set; }
        public decimal? TotalCurrentPeriodUsd { get; set; }

        public decimal? TotalLastPeriodIdr { get; set; }
        public decimal? TotalLastPeriodUsd { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal? ProcessOID { get; set; }
        public string ApprovalAction { get; set; }
        public string ApprovalRemarks { get; set; }
        public string ApprovalType { get; set; }
        public string UpdateType { get; set; }
    }
}