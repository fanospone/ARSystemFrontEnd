using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
    public class vmARPaymentInvoiceTower : BaseClass
    {
        public vmARPaymentInvoiceTower()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmARPaymentInvoiceTower(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int trxInvoiceHeaderID { get; set; }
        public string InvNo { get; set; }

        public string InvTemp { get; set; }
        public DateTime? InvPrintDate { get; set; }
        public string Term { get; set; }
        public decimal? InvSumADPP { get; set; }
        public decimal? InvTotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? InvTotalAPPN { get; set; }
        public decimal? InvTotalAPPH { get; set; }
        public decimal? InvTotalPenalty { get; set; }
        public DateTime? InvReceiptDate { get; set; }
        public string Currency { get; set; }
        public int AgingDays { get; set; }

        public string InvInternalPIC { get; set; }
        public string PaidStatus { get; set; }
        
        public string InvReceiptFile { get; set; }
        public DateTime? ChecklistDate { get; set; }
        public int? mstInvoiceStatusId { get; set; }
        public int? mstInvoiceCategoryId { get; set; }
        public string InvOperatorID { get; set; }
        public string InvCompanyId { get; set; }
        public string InvoiceTypeId { get; set; }
        public string Company { get; set; }
        public decimal? PartialPaid { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public int PPHIndex { get; set; }
        public int PPEIndex { get; set; }
        public int PPFIndex { get; set; }
        public bool? isPPHFinal { get; set; }
        public string PPHType { get; set; }

        public List<vwInvoiceTowerDetail> Detail { get; set; }

        public DateTime? InvPaidDate { get; set; }

        public int? mstPaymentId { get; set; }

        public string InvPaidStatus { get; set; }
        public decimal? PAM { get; set; }
        public decimal? RND { get; set; }
        public decimal? DBT { get; set; }
        public decimal? RTGS { get; set; }
        public decimal? PNT { get; set; }
        public decimal? PPE { get; set; }
        public decimal? PPH { get; set; }
        public decimal? PAT { get; set; }
        public decimal? PPF { get; set; }
        public decimal? ARTotalPaid { get; set; }
        public List<vwARPaymentInvoiceTowerHistory> History { get; set; }
        public decimal? InvAmountLossPPN { get; set; }
        public trxInvoiceMatchingAR InvoiceMatchingAR { get; set; }
        public string CompanyCode { get; set; }
    }
}
