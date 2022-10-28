using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmInvoiceProduction : BaseClass
    {
        public vmInvoiceProduction()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmInvoiceProduction(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string Type { get; set; }
        public int Value { get; set; }
        
        //Header
        public string InvoiceNumber { get; set; }
        public string SubjectInvoice { get; set; }
        public decimal AmountInvoice { get; set; }
        public string Operator { get; set; }
        public string Company { get; set; }
        public string TypeInvoice { get; set; }
        public DateTime? CreateInvoice { get; set; }
        public DateTime? PostingInvoice { get; set; }
        public DateTime? SubmitDocInvoice { get; set; }
        public DateTime? ApproveDocInvoice { get; set; }
        public DateTime? ReceiptDocInvoice { get; set; }
        public string TypePayment { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string DocumentPayment { get; set; }
        public DateTime? DocumentPaymentIntegration { get; set; }

        //Detail
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteIDOpr { get; set; }
        public string SiteNameOpr { get; set; }
        public string BapsType { get; set; }
        public DateTime? StartPeriodInvoice { get; set; }
        public DateTime? EndPeriodInvoice { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? CreateInvoiceDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public DateTime? SubmitChecklistDate { get; set; }
        public DateTime? ApproveChecklistDate { get; set; }
    }
}
