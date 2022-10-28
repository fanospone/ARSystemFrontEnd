using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmSaveRTIPartition : BaseClass
    {
        public vmSaveRTIPartition()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmSaveRTIPartition(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string State { get; set; }
        public int ID { get; set; }
        public int trxReconcileID { get; set; }
        public int Term { get; set; }
        public int TermPartitionID { get; set; }
        public DateTime? StartPeriodInvoiceDate { get; set; }
        public DateTime? EndPeriodInvoiceDate { get; set; }
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        //public decimal? AmountIDR { get; set; }
        //public decimal? AmountUSD { get; set; }
        public string CustomerID { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; } 
        public decimal? InflationAmount { get; set; }
        public decimal? AdditionalAmount { get; set; }
        public decimal? DeductionAmount { get; set; }
        public decimal? PenaltySlaAmount { get; set; }
        public string UserID { get; set; }

    }
}
