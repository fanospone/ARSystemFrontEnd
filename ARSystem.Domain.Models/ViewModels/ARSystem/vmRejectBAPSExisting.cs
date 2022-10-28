using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmRejectBAPSExisting : BaseClass
    {
        public vmRejectBAPSExisting()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmRejectBAPSExisting(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string SONumber { get; set; }
        public string BapsType { get; set; }
        public string StipSiro { get; set; }
        public string PeriodNo { get; set; }
        public string BapsNo { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string StsRknHarga { get; set; }
        public string StsRknWaktu { get; set; }
        public string StsBauk { get; set; }
        public string StsSpk { get; set; }
        public string StsPo { get; set; }
        public string StsMom { get; set; }
        public string StsKontrak { get; set; }
        public string UserID { get; set; }

        public decimal AmountRental { get; set; }
        public decimal AmountService { get; set; }
        public string InvoiceTypeId { get; set; }
        public decimal AmountOverdaya { get; set; }
        public decimal AmountOverblast { get; set; }
        public decimal AmountPenalty { get; set; }
        public decimal AmountDiscount { get; set; }

    }
}
