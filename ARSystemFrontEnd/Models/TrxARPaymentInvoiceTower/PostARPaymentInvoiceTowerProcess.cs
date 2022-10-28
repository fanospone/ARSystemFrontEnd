using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Models
{
    public class PostARPaymentInvoiceTowerProcess : DatatableAjaxModel
    {
        public int trxInvoiceHeaderId { get; set; }
        public int mstInvoiceCategoryId { get; set; }
        public string InvPaidDate { get; set; }
        public int mstPaymentId { get; set; }
        public string InvPaidStatus { get; set; }
        public decimal PAM { get; set; }
        public decimal RND { get; set; }
        public decimal DBT { get; set; }
        public decimal RTGS { get; set; }
        public decimal PNT { get; set; }
        public decimal PPE { get; set; }
        public decimal PPH { get; set; }
        public decimal PAT { get; set; }
        public decimal PPF { get; set; }
        public decimal ARTotalPaid { get; set; }
        public bool IsPPHFInal { get; set; }
        public string strRemarks { get; set; }
        public trxInvoiceMatchingAR InvoiceMatchingAR { get; set; }
        public string vDocumentPayment { get; set; }
        public string vCompanyCode { get; set; }
        public string vTglUangMasuk { get; set; }
    }
}