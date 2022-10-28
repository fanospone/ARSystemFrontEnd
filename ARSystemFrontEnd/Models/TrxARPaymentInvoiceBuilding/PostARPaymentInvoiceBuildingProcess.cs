using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostARPaymentInvoiceBuildingProcess
    {
        public int trxInvoiceHeaderId { get; set; }
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
        public decimal ARTotalPaid { get; set; }
        public decimal PPF { get; set; }
        public bool IsPPHFinal{ get; set; }
        public string strRemarks { get; set; }
    }
}