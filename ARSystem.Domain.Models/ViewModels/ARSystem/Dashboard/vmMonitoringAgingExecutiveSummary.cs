using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmMonitoringAgingExecutiveSummary : BaseClass
    {
        public vmMonitoringAgingExecutiveSummary()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmMonitoringAgingExecutiveSummary(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string OperatorID { get; set; }
        public decimal AmountCurrent { get; set; }
        public decimal AmountOD_30 { get; set; }
        public decimal AmountOD_60 { get; set; }
        public decimal AmountOD_90 { get; set; }
        public decimal TotalOS { get; set; }
        public decimal PercetageODPerOpt { get; set; }
        public decimal PercentageODAllOpt { get; set; }

        //Detail
        public string CompanyID { get; set; }
        public string BucketAging { get; set; }
        public decimal aOutstandingGross { get; set; }
        public decimal aOutstandingNett { get; set; }
        public decimal AmountBankOut { get; set; }
        public decimal PAM { get; set; }
        public string InvoiceType { get; set; }
        public string PowerType { get; set; }
        public string TenantType { get; set; }
        public string STIPCode { get; set; }
        public DateTime ARCApproveDate { get; set; }
        public DateTime PaidDate { get; set; }
        public string PaidStatus { get; set; }
        public string InvNo { get; set; }
    }
}
